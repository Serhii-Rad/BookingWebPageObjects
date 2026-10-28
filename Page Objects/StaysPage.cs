using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Sub_panels;
using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Pages
{
    internal class StaysPage : PageBase
    {
        public StaysPage(IPage page) : base(page)
        {
        }

        public InputField DestinationInputField => new InputField(_pageLocator.Locator("input[id='searchbox-horizontal-destination-input']"));
        public DropDownMenu TrendingDestinationsDropDown => new DropDownMenu(_pageLocator.Locator("div[data-testid='autocomplete-results']"), "li[role='option']");
        public Button SearchButton => new Button(_pageLocator.Locator("button[data-testid='searchbox-submit-button']"));
        public Button DatesButton => new Button(_pageLocator.Locator("button[data-testid='searchbox-dates-container']"));
        public Button OccupancyButton => new Button(_pageLocator.Locator("button[data-testid='occupancy-config']"));

        public SelectDatesPanel SelectDatesPanel => new SelectDatesPanel(_pageLocator, DatesButton);
        public OccupancyPanel OccupancyDropDown => new OccupancyPanel(_pageLocator, OccupancyButton);
        public CheckBox TravelingForWorkCheckBox => new CheckBox(_pageLocator.Locator("input[name='sb_travel_purpose']"));
        public CheckBox AddFlightsToMySearchCheckBox => new CheckBox(_pageLocator.Locator("input[name='sb_flight_search']"));
    }
}
