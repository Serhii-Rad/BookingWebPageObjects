using BookingWebPageObjects.BaseElements;
using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;
using PlaywrightBaseFramework.PageCore;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.Sub_panels
{
    internal class OccupancyPanel : SubPanelBase
    {
        private static readonly string panelSelector = "div[data-testid='occupancy-popup']";

        public OccupancyPanel(ILocator pageLocator, ButtonBase triggerToOpen) : base(pageLocator.Locator(panelSelector), triggerToOpen)
        {
        }

        public InputFieldWithButtons AdultsInputs => new InputFieldWithButtons(_panelLocator.Locator("input[id='group_adults']"));
        public InputFieldWithButtons ChildrenInputs => new InputFieldWithButtons(_panelLocator.Locator("input[id='group_children']"));
        public InputFieldWithButtons RoomsInputs => new InputFieldWithButtons(_panelLocator.Locator("input[id='no_rooms']"));

        public DropDownMenuBase GetChildAgeDropDown(int number = 0) => new DropDownMenuBase(_panelLocator.Locator("select[name='age']").Nth(number), "option");

        public ButtonBase DoneButton => new ButtonBase(_panelLocator.Locator("button").GetByText("Done"));

        public CheckBoxBase TravelingWithPetsCheckBox => new CheckBoxBase(_panelLocator.Locator("input[id='pets'][role='switch']~label"));

        public async Task OpenAsync()
        {
            Logs.LogMethodEntry($"Locator: {_panelLocator}", logLevel: CoreLogLevel.Info);
            if (await IsHiddenAsync())
            {
                if (_triggerElementToOpen is ButtonBase button)
                    await button.ClickAsync();
                else
                    throw new InvalidOperationException("Button to open is null or not a Button.");
            }
        }
    }
}
