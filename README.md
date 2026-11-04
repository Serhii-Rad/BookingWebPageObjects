# BookingWebPageObjects

## What this project is for

`BookingWebPageObjects` is the application-specific layer of the automation solution. It describes parts of the Booking.com website as page objects and smaller UI components, then uses them from NUnit smoke tests.

This project depends on `PlaywrightBaseFramework`, which provides reusable Playwright actions and base element classes. That project depends on `TestFrameworkCore`, which provides shared interfaces, logging, and helpers.

```text
Smoke tests and NUnit fixture
             ↓
Booking page objects, panels, and Booking-specific elements
             ↓
PlaywrightBaseFramework (ILocator, IPage, base wrappers)
             ↓
TestFrameworkCore (shared contracts and helpers)
             ↓
Microsoft.Playwright
```

The main design rule is to keep each kind of knowledge in the right place:

| Layer | Knows about | Example |
| --- | --- | --- |
| Test | The behavior being checked and expected outcome | “Selecting USD updates the currency shown in the header.” |
| Page object | Booking page structure and the elements on that page | `StaysPage.SearchButton` |
| Panel/component | A reusable region and its user operations | `OccupancyPanel.OpenAsync()` |
| Element wrapper | A single kind of control | `Button.ClickAsync()` or `InputField.SetTextAsync()` |
| Playwright base class | Generic browser mechanics | Click, wait, type, inspect locator |

This separation makes tests read more like user actions and gives selectors one clear home.

## How to read the examples

The code in this README follows the current project source. The page objects and the tests are in the same test assembly and most classes are `internal`. They are meant to be used inside this project; another assembly cannot access those classes unless they are made `public`.

The project targets .NET 9.0 and is an NUnit test project. `BookingSuiteBase` reads `baseUrl`, `defaultUserName`, and `defaultPassword` from [NUnit test parameters](https://docs.nunit.org/articles/nunit/writing-tests/TestContext.html#testparameters) when the fixture type is initialized. At present, only `baseUrl` is used by the default setup; the credentials are placeholders for future sign-in flows. If `baseUrl` is missing, the default setup does not navigate to the website.

For `dotnet test`, a `.runsettings` file can provide `baseUrl` through `TestRunParameters`:

```xml
<RunSettings>
  <TestRunParameters>
    <Parameter name="baseUrl" value="https://www.booking.com/" />
  </TestRunParameters>
</RunSettings>
```

Pass that file to `dotnet test` with `--settings path/to/booking.runsettings`. See [Microsoft's runsettings documentation](https://learn.microsoft.com/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file) for test parameters. Do not put real account passwords in a checked-in settings file; supply secrets through your runner or CI secret configuration.

## 1. Start with an element wrapper

Booking-specific element classes are thin subclasses of the generic wrappers in `PlaywrightBaseFramework`. For example:

```csharp
internal class Button : ButtonBase
{
    public Button(ILocator locator) : base(locator) { }
}
```

`ButtonBase` already knows how to click, hover, right-click, double-click, and read text. The Booking `Button` gives page objects a consistent local type and leaves room for Booking-only behavior later.

The other wrappers follow the same idea:

- `InputField : InputFieldBase`
- `CheckBox : CheckBoxBase`
- `DropDownMenu : DropDownMenuBase`
- `StaticMenu : StaticMenuBase`

Use a base wrapper when the control behaves like the generic control. Create a specialized wrapper when the application control needs different behavior, as `InputFieldWithButtons` does for the adult, child, and room counters.

## 2. Build a page object from locators and controls

A page object represents a page or a stable region of a page. It owns the selectors and exposes useful controls as properties. Tests use those properties instead of repeating CSS selectors.

`StaysPage` derives from `PageBase` and scopes its child selectors under the page's `main` element:

```csharp
internal class StaysPage : PageBase
{
    public StaysPage(IPage page) : base(page.Locator("main")) { }

    public Button SearchButton => new Button(
        _pageLocator.Locator("button[data-testid='searchbox-submit-button']"));
}
```

The real class also exposes the destination input, autocomplete list, date and occupancy triggers, panels, and search checkboxes.

The current page-object properties are:

| Page object | Properties |
| --- | --- |
| `PageMain` | `TabsMenu`, `CurrencyPanel`, `LanguagePanel`, `CurrencyButton`, `LanguageButton`, `CustomerServiceButton`, `ListYourPropertyButton`, `UserProfileButton`, `UserDropDownMenu` |
| `StaysPage` | `DestinationInputField`, `TrendingDestinationsDropDown`, `SearchButton`, `DatesButton`, `OccupancyButton`, `SelectDatesPanel`, `OccupancyDropDown`, `TravelingForWorkCheckBox`, `AddFlightsToMySearchCheckBox` |
| `Pages` | `PageMain`, `StaysPage` |

### Why scope selectors to a page root?

`PageBase` stores a root locator in `_pageLocator`. Calling `_pageLocator.Locator("...")` searches inside that root, which can make selectors more specific and reduce collisions with similar controls elsewhere on the page. `PageMain` uses `body`; `StaysPage` uses `main`.

Locators are Playwright queries, not snapshots of DOM elements. They are resolved when an action runs, so a stored `ILocator` can continue to work when the page rerenders. Prefer accessible roles and stable test IDs when the site provides them. This project currently uses a mixture of `data-testid`, IDs, roles, and CSS selectors.

### Lazy properties with `??=`

`PageMain` and `StaysPage` use properties such as:

```csharp
private Button? _searchButton;

public Button SearchButton => _searchButton ??= new Button(
    _pageLocator.Locator("button[data-testid='searchbox-submit-button']"));
```

The first property access creates the wrapper and stores it. Later accesses return the same wrapper. This is called lazy initialization: create an object only when it is first needed.

The wrapper contains a locator, and the locator resolves against the current DOM when used. Caching the wrapper therefore does not freeze the current HTML element.

## 3. Put repeated UI regions into panel classes

The header's currency picker and the search form's date and occupancy pickers are separate panels. They have their own selectors and actions, so each is represented by a class instead of mixing its logic into the page class.

### `SubPanelBase`

`SubPanelBase` derives from `PageBase` and stores:

- `_panelLocator`: the locator for the panel itself.
- `_triggerElementToOpen`: an optional `IElementBase` that opens the panel.

It has one constructor for a panel with a unique locator and another for a panel that is associated with a trigger. The base class centralizes the relationship; each concrete panel defines how it opens and what actions it offers.

### `CurrencyPanel` and `LanguagePanel`

`PageMain` exposes the trigger buttons and their panels:

```csharp
public Button CurrencyButton => ...;
public CurrencyPanel CurrencyPanel =>
    _currencyPanel ??= new CurrencyPanel(_pageLocator, CurrencyButton);
```

The panel receives both a page-root locator and the button that opens it. Its `OpenAsync()` checks whether the panel is hidden and clicks the trigger only when needed. This makes `OpenAsync()` safe to call before an operation without blindly toggling an already-open panel.

`CurrencyPanel` provides:

- `OpenAsync()` and `CloseAsync()`.
- `SelectCurrency(string currencyCode)`.
- `GetSelectedCurrency()`.
- `IsVisibleCurrencyOption(string currencyCode)`.

`LanguagePanel` provides the matching `OpenAsync()` / `CloseAsync()` operations and `SelectLanguage(string language)`, `GetSelectedLanguage()`, and `IsVisibleLanguageOption(string language)`.

Example:

```csharp
await PageMain.CurrencyPanel.OpenAsync();
string selected = await PageMain.CurrencyPanel.GetSelectedCurrency();

if (selected == "USD")
    await PageMain.CurrencyPanel.SelectCurrency("EUR");
```

Prefer an operation such as `SelectCurrency` in the test to a raw click on a deeply nested option. The operation hides how that option is located and keeps that detail in one place.

### `SelectDatesPanel`

`StaysPage.SelectDatesPanel` represents the calendar panel. Its public operations are:

- `OpenAsync()` opens it through the `DatesButton` trigger if it is hidden.
- `SelectDates(DateTime checkInDate, DateTime checkOutDate)` selects both dates using the calendar's accessible labels.
- `GetSelectedDatesAsync()` returns a tuple `(DateTime? checkInDate, DateTime? checkOutDate)` read from the selected calendar elements' `data-date` attributes.

Example:

```csharp
DateTime checkIn = DateTime.Today.AddDays(7);
DateTime checkOut = checkIn.AddDays(7);

await StaysPage.SelectDatesPanel.SelectDates(checkIn, checkOut);
var selected = await StaysPage.SelectDatesPanel.GetSelectedDatesAsync();

Assert.That(selected.checkInDate, Is.EqualTo(checkIn));
Assert.That(selected.checkOutDate, Is.EqualTo(checkOut));
```

The current selector builds the calendar label using the current machine culture. The fixture sets the browser context locale to `en-US`, but the .NET test process culture is a separate setting. If date selection becomes flaky on another machine, make the label formatting culture explicit.

### `OccupancyPanel`

`OccupancyPanel` groups the controls inside the occupancy popup:

- `AdultsInputs`, `ChildrenInputs`, and `RoomsInputs` are `InputFieldWithButtons` wrappers.
- `GetChildAgeDropDown(int number = 0)` returns the age `<select>` for the requested child index.
- `TravelingWithPetsCheckBox` wraps the pets switch.
- `DoneButton` wraps the Done action.
- `OpenAsync()` opens the panel through the occupancy button if it is hidden.

Example:

```csharp
await StaysPage.OccupancyDropDown.OpenAsync();
await StaysPage.OccupancyDropDown.AdultsInputs.SetTextAsync("3");
await StaysPage.OccupancyDropDown.GetChildAgeDropDown().SelectItemAsync("10");
await StaysPage.OccupancyDropDown.DoneButton.ClickAsync();
```

`InputFieldWithButtons.SetTextAsync` is specialized for numeric steppers: it reads the current number, parses the target number, then clicks plus or minus until it reaches the target. It does not type into the input. The method hides (`new`) the inherited `SetTextAsync`; therefore call it through the `InputFieldWithButtons` type to get stepper behavior. Its other inherited-looking parameters (`withEnterPress`, clearing, and typing delay) do not control the stepper loop. The current implementation also requires the current and target text to be valid integers.

## 4. Group page objects behind `Pages`

`Pages` is a small facade that groups the main site page objects:

```csharp
public PageMain PageMain => Pages.PageMain;
public StaysPage StaysPage => Pages.StaysPage;
```

This keeps tests convenient: a test can start with `StaysPage.SearchButton` instead of constructing each page object itself. Inside `Pages`, `PageMain` and `StaysPage` are lazily initialized and cached.

There is a current caching mismatch: `BookingSuiteBase.Pages` returns `new Pages(Page)` every time it is accessed. That means the `Pages` facade and its internal cached page objects are recreated on each access. To preserve the intended cache, keep one facade per test page:

```csharp
private Pages? _pages;
public Pages Pages => _pages ??= new Pages(Page);
```

Reset `_pages` when a new `Page` is created. Otherwise the facade could continue to refer to an earlier page.

## 5. Understand the NUnit fixture lifecycle

`BookingSuiteBase` derives from `SuiteBase` and connects the generic Playwright lifecycle to Booking-specific startup:

1. `SuiteBase.BaseOneTimeSetup()` configures logs and creates the download directory.
2. `SuiteBase.BaseEveryTestSetupAsync()` launches Chromium and creates a browser context.
3. `BookingSuiteBase.BookingEveryTestSetUp()` creates a page, finds a matching startup method, and invokes it.
4. `DefaultTestSetUpAsync()` navigates to `BaseUrl` (when provided) and dismisses the sign-in promotion if it appears.
5. The test runs.
6. Booking-specific teardown runs, then `SuiteBase` handles the result, attaches artifacts, and closes the page/context.

The order matters: page objects need `Page`, so they should be used after `CreatePageAsync()` in setup. When adding tests to this project, derive from `BookingSuiteBase`; its setup already creates the page and invokes the selected Booking startup method:

```csharp
[TestFixture]
internal sealed class MyBookingTests : BookingSuiteBase
{
    [Test]
    public async Task SearchButtonIsVisible()
    {
        Step = 1;
        Assert.That(await StaysPage.SearchButton.IsVisibleAsync(), Is.True);
    }
}
```

If a different project derives directly from `SuiteBase`, it must provide its own per-test setup that creates a page and invokes the desired startup logic.

### Per-test startup methods

`CurrentTestId` is extracted from a test method name matching `TC` followed by digits. A method marked with `TestStartUpAttribute` can replace the default startup for that ID:

```csharp
[TestStartUp("TC0010")]
private async Task StartOccupancyScenarioAsync()
{
    await Page.GotoAsync("https://www.booking.com/");
    // Set up a state specific to TC0010.
}
```

`BookingEveryTestSetUp()` calls `GetCustomStartUpMethod()` and awaits the returned task. If no method matches the current ID, it uses the default `DefaultTestSetUpAsync()` method. The attribute value must exactly match the extracted ID, including the `TC` prefix. Avoid defining multiple startup methods for the same ID because the lookup expects a single match.

The base teardown similarly invokes a matching method marked with `TestCleanupAttribute("TC0010")`. These attributes are project conventions implemented by the fixture; NUnit does not run these custom attributes on its own.

## 6. Write tests as arrange, act, and assert

A smoke test should make three things easy to find:

1. **Arrange:** choose the target state or data.
2. **Act:** use page-object operations to perform a user action.
3. **Assert:** check the visible result or application state.

Example based on the currency smoke test:

```csharp
[Test]
public async Task SelectingCurrencyUpdatesTheHeader()
{
    Step = 1; // Open the panel and read its current selection.
    await PageMain.CurrencyPanel.OpenAsync();
    string current = await PageMain.CurrencyPanel.GetSelectedCurrency();

    string target = current == "USD" ? "EUR" : "USD";

    Step = 2; // Act through the page object.
    await PageMain.CurrencyPanel.SelectCurrency(target);

    Step = 3; // Assert the user-visible result.
    Assert.That(
        await PageMain.CurrencyButton.GetTextAsync(),
        Is.EqualTo(target),
        "The currency shown in the header should match the selected currency.");
}
```

The `Step` property writes to `Logs.CurrentStep`, so framework log messages and failure handling can include the current step number. Use short step descriptions in comments or log messages so the test output is useful when a test fails.

### Check multiple independent results

NUnit 5 provides `Assert.EnterMultipleScope()` when several independent checks should all run and report their failures together:

```csharp
using (Assert.EnterMultipleScope())
{
    Assert.That(actualCheckIn, Is.EqualTo(expectedCheckIn));
    Assert.That(actualCheckOut, Is.EqualTo(expectedCheckOut));
    Assert.That(actualRoomCount, Is.EqualTo(expectedRoomCount));
}
```

Use this when each check is useful even if another one fails. Keep dependent actions outside the scope: if the test cannot safely continue after a failed action, assert immediately before using its result.

## 7. Reuse test patterns without hiding what the test verifies

### Menu item factory

`StaticMenuBase` returns locators for items, while the page-object layer decides which wrapper to build:

```csharp
IReadOnlyList<Button> tabs = await PageMain.TabsMenu
    .GetMenuElementsAsync(locator => new Button(locator));

var labels = new List<string?>();
foreach (Button tab in tabs)
    labels.Add(await tab.GetTextAsync());
```

The factory expression is a small adapter: it converts each Playwright locator into the application wrapper used by this project. This avoids making the Core menu interface depend on Playwright.

### Reusable small actions

When a workflow is repeated, give it a descriptive method in the page or panel class. For example, a future `SearchForStayAsync(destination, checkIn, checkOut)` could coordinate the destination field, calendar, and search button. Keep the assertions in the test so the test still states what outcome matters.

Avoid creating a generic helper for a one-off action if it makes the test harder to understand. A good page-object method has a name in the user's language, hides selector details, and does one coherent UI task.

## Existing project map

| Folder | Main types | Responsibility |
| --- | --- | --- |
| `Page Objects` | `Pages`, `PageMain`, `StaysPage` | Page roots, Booking selectors, and page-level components |
| `Sub panels` | `SubPanelBase`, `CurrencyPanel`, `LanguagePanel`, `SelectDatesPanel`, `OccupancyPanel` | Popup/panel selectors and their user operations |
| `BaseElements` | `Button`, `CheckBox`, `DropDownMenu`, `InputField`, `InputFieldWithButtons`, `StaticMenu` | Booking-specific wrappers over Playwright base elements |
| `TestSuites` | `BookingSuiteBase`, `SmokeTests` | NUnit lifecycle, site startup, and sample behavior tests |
| `Helpers` | `WaitHelper` | Booking-level wrapper around the shared wait helper |

## Current limitations to keep in mind

- The tests exercise the live Booking.com site. Site markup, displayed text, availability, region, locale, account state, and cookie/promotional dialogs may change independently of this code.
- `TC0008` expects a profile button that may not exist when the browser is not signed in; its source comment already notes this.
- `TC0004` currently asserts that the language button text is empty (`Is.True` for `string.IsNullOrEmpty`), even though the test message says the current language should be displayed. Review that expected condition before treating the test as a reliable specification.
- Several smoke tests use fixed `WaitForTimeoutAsync(1500)` pauses. Prefer waiting for a specific state or selected value so tests do not depend on a guessed animation duration.
- `BookingSuiteBase.Pages` currently constructs a new `Pages` facade on each access, so its advertised page-object cache is not retained. The sample above shows the per-test cache pattern.
- `WaitHelper` currently adds no behavior beyond inheriting `WaitHelperBase`; use it only if Booking-specific waiting behavior is added.
- `InputFieldWithButtons.SetTextAsync` hides a base method and ignores several parameters with the same names. Keep this in mind when passing the specialized control around as its base type.

## Suggested learning path

1. Read `TC0002_TestCurrencyButtonFunctionality` in `SmokeTests.cs`: observe the test calling a page property, then an element method.
2. Follow `PageMain.CurrencyButton` to see where the selector is declared.
3. Follow `Button` to `ButtonBase` to see where generic click/text behavior lives.
4. Read `CurrencyPanel` to see how a multi-control component has its own operations.
5. Read `TC0009_TestCheckInOutDates` and `TC0010_TestOccupancy` for examples involving multiple steps and multiple assertions.
6. Add a new test by first identifying the page or panel that owns the control, then adding a locator property there, and finally writing a test that performs a user action and checks its result.
