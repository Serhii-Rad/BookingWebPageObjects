using BookingWebPageObjects.BaseElements;
using Microsoft.Playwright;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.Sub_panels
{
    internal class OccupancyPanel : SubPanelBase
    {
        private static readonly string panelSelector = "div[data-testid='occupancy-popup']";

        public OccupancyPanel(ILocator pageLocator, Button triggerToOpen) : base(pageLocator.Locator(panelSelector), triggerToOpen)
        {
        }

        public InputFieldWithButtons AdultsInputs => new InputFieldWithButtons(_panelLocator.Locator("input[id='group_adults']"));
        public InputFieldWithButtons ChildrenInputs => new InputFieldWithButtons(_panelLocator.Locator("input[id='group_children']"));
        public InputFieldWithButtons RoomsInputs => new InputFieldWithButtons(_panelLocator.Locator("input[id='no_rooms']"));

        public DropDownMenu GetChildAgeDropDown(int number = 0) => new DropDownMenu(_panelLocator.Locator("select[name='age']").Nth(number), "option");

        public Button DoneButton => new Button(_panelLocator.Locator("button").GetByText("Done"));

        public CheckBox TravelingWithPetsCheckBox => new CheckBox(_panelLocator.Locator("input[id='pets'][role='switch']~label"));

        public async Task OpenAsync()
        {
            Logs.LogMethodEntry(logLevel: Microsoft.Testing.Platform.Logging.LogLevel.Information);
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
