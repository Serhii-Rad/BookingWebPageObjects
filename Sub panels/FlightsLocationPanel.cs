using BookingWebPageObjects.BaseElements;
using Microsoft.Playwright;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.Sub_panels
{
    /// <summary>
    /// Airport/city autocomplete popup on the Flights page. The same popup is used for both "Leaving from" and "Going to" (only one is open at a time).
    /// </summary>
    internal class FlightsLocationPanel : SubPanelBase
    {
        private static readonly string panelSelector = "div[data-bui-trap-root]:has(input[data-ui-name='input_text_autocomplete'])";
        private static readonly string suggestionSelector = "li[data-ui-name='locations_list_item']";

        public FlightsLocationPanel(ILocator pageLocator, Button triggerElementToOpen) : base(pageLocator.Locator(panelSelector), triggerElementToOpen)
        {
        }

        public InputField SearchInputField => new InputField(_panelLocator.Locator("input[data-ui-name='input_text_autocomplete']"));
        public DropDownMenu SuggestionsDropDown => new DropDownMenu(_panelLocator.Locator("ul[role='listbox']"), suggestionSelector);
        /// <summary>
        /// Chips of already selected airports/cities (ex. 'IEV Kyiv'). Origin is prefilled by geolocation.
        /// </summary>
        public ILocator SelectedLocationsRemoveButtons => _panelLocator.Locator("button[aria-label$='Remove airport or city']");

        /// <summary>
        /// Removes previously selected locations, types the search text and clicks the first suggestion that contains suggestionText (or the first suggestion if it is null).
        /// </summary>
        public async Task SelectLocationAsync(string searchText, string? suggestionText = null)
        {
            Logs.LogMethodEntry($"Search: '{searchText}'; Suggestion: '{suggestionText}'", CoreLogLevel.Info);

            await OpenAsync();
            await ClearSelectedLocationsAsync();
            await SearchInputField.SetTextAsync(searchText, withEnterPress: false, millisecondsInputDelay: 100);

            var suggestionLocator = _panelLocator.Locator(suggestionSelector);
            if (suggestionText != null)
                suggestionLocator = suggestionLocator.Filter(new() { HasText = suggestionText });
            await suggestionLocator.First.ClickAsync();
        }

        public async Task ClearSelectedLocationsAsync()
        {
            Logs.LogMethodEntry(logLevel: CoreLogLevel.Info);

            // chips are rendered together with the search input - wait for it, otherwise CountAsync returns 0 right after opening
            await SearchInputField.IsVisibleAsync();
            while (await SelectedLocationsRemoveButtons.CountAsync() > 0)
                await SelectedLocationsRemoveButtons.First.ClickAsync();
        }

        public async Task OpenAsync()
        {
            Logs.LogMethodEntry(logLevel: CoreLogLevel.Info);
            if (await IsHiddenAsync())
            {
                if (_triggerElementToOpen is Button button)
                    await button.ClickAsync();
                else
                    throw new InvalidOperationException("Button to open is null or not a Button.");
            }
        }
    }
}
