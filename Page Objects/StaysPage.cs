using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Sub_panels;
using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Pages
{
    internal class StaysPage : PageBase
    {
        private static readonly string _pageSelector = "main";

        private InputField? _destinationInputField;
        private DropDownMenu? _trendingDestinationsDropDown;
        private Button? _searchButton;
        private Button? _datesButton;
        private Button? _occupancyButton;
        private SelectDatesPanel? _selectDatesPanel;
        private OccupancyPanel? _occupancyDropDown;
        private CheckBox? _travelingForWorkCheckBox;
        private CheckBox? _addFlightsToMySearchCheckBox;

        public StaysPage(IPage page) : base(page.Locator(_pageSelector))
        {
        }

        public InputField DestinationInputField => _destinationInputField ??= new InputField(_pageLocator.Locator("input[id='searchbox-horizontal-destination-input']"));
        public DropDownMenu TrendingDestinationsDropDown => _trendingDestinationsDropDown ??= new DropDownMenu(_pageLocator.Locator("div[data-testid='autocomplete-results']"), "li[role='option']");
        public Button SearchButton => _searchButton ??= new Button(_pageLocator.Locator("button[data-testid='searchbox-submit-button']"));
        public Button DatesButton => _datesButton ??= new Button(_pageLocator.Locator("button[data-testid='searchbox-dates-container']"));
        public Button OccupancyButton => _occupancyButton ??= new Button(_pageLocator.Locator("button[data-testid='occupancy-config']"));

        public SelectDatesPanel SelectDatesPanel => _selectDatesPanel ??= new SelectDatesPanel(_pageLocator, DatesButton);
        public OccupancyPanel OccupancyDropDown => _occupancyDropDown ??= new OccupancyPanel(_pageLocator, OccupancyButton);
        public CheckBox TravelingForWorkCheckBox => _travelingForWorkCheckBox ??= new CheckBox(_pageLocator.Locator("input[name='sb_travel_purpose']"));
        public CheckBox AddFlightsToMySearchCheckBox => _addFlightsToMySearchCheckBox ??= new CheckBox(_pageLocator.Locator("input[name='sb_flight_search']"));
    }
}
