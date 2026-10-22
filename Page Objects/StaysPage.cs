using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Sub_panels;
using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Pages
{
    internal class StaysPage : PageBase
    {
        public StaysPage(IPage page) : base(page)
        {
        }

        public InputField DestinationInputField => new InputField(_pageLocator.Locator("input[id='searchbox-horizontal-destination-input']"));
        public DropDownMenu TrendingDestinationsDropDown => new DropDownMenu(_pageLocator.Locator("div[data-testid='autocomplete-results']"));
        public DateTimeCalendar SelectDatesDateCalendar => new DateTimeCalendar(_pageLocator.Locator("button[data-testid='searchbox-dates-container']"));
        public Button SearchButton => new Button(_pageLocator.Locator("button[data-testid='searchbox-submit-button']"));
        public OccupancyDropDownPanel OccupancyDropDown => new OccupancyDropDownPanel(_pageLocator);
        public CheckBox TravelingForWorkCheckBox => new CheckBox(_pageLocator.Locator("input[name='sb_travel_purpose']"));
        public CheckBox AddFlightsToMySearchCheckBox => new CheckBox(_pageLocator.Locator("input[name='sb_flight_search']"));
    }
}
