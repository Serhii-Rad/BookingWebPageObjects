using BookingWebPageObjects.BaseElements;
using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.Sub_panels
{
    /// <summary>
    /// Travelers popup on the Flights page. Unlike Stays there are no rooms and no pets.
    /// </summary>
    internal class FlightsTravelersPanel : SubPanelBase
    {
        private static readonly string panelSelector = "div[data-bui-trap-root]:has([data-ui-name='occupancy_adults'])";

        public FlightsTravelersPanel(ILocator pageLocator, Button triggerToOpen) : base(pageLocator.Locator(panelSelector), triggerToOpen)
        {
        }

        public InputFieldWithButtons AdultsInputs => new InputFieldWithButtons(_panelLocator.Locator("div[data-ui-name='occupancy_adults'] input[role='spinbutton']"));
        public InputFieldWithButtons ChildrenInputs => new InputFieldWithButtons(_panelLocator.Locator("div[data-ui-name='occupancy_children'] input[role='spinbutton']"));

        public Button DoneButton => new Button(_panelLocator.Locator("button[data-ui-name='button_occupancy_action_bar_done']"));

        private ILocator GetChildAgeSelect(int number) => _panelLocator.Locator($"select[data-ui-name='select_occupancy_children_age_{number}']");

        /// <summary>
        /// Not via DropDownMenu: DropDownMenuBase.SelectItemAsync clicks the native select first, the opened native list freezes the Flights page.
        /// </summary>
        public async Task SelectChildAgeAsync(int age, int number = 0)
        {
            Logs.LogMethodEntry($"Child number: {number}; Age: {age}", CoreLogLevel.Info);

            await GetChildAgeSelect(number).SelectOptionAsync(new SelectOptionValue { Value = age.ToString() });
        }

        public async Task<string?> GetChildAgeAsync(int number = 0)
        {
            Logs.LogMethodEntry($"Child number: {number}");

            return await GetChildAgeSelect(number).InputValueAsync();
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
