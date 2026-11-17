using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Sub_panels;
using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Pages
{
    internal class StaysPage : PageBase
    {
        private static readonly string _pageSelector = "main";

        private InputFieldBase? _destinationInputField;
        private DropDownMenuBase? _trendingDestinationsDropDown;
        private ButtonBase? _searchButton;
        private ButtonBase? _datesButton;
        private ButtonBase? _occupancyButton;
        private SelectDatesPanel? _selectDatesPanel;
        private OccupancyPanel? _occupancyDropDown;
        private CheckBoxBase? _travelingForWorkCheckBox;
        private CheckBoxBase? _addFlightsToMySearchCheckBox;

        public StaysPage(IPage page) : base(page.Locator(_pageSelector))
        {
        }

        public InputFieldBase DestinationInputField => _destinationInputField ??= new InputFieldBase(_pageLocator.Locator("input[id='searchbox-horizontal-destination-input']"));
        public DropDownMenuBase TrendingDestinationsDropDown => _trendingDestinationsDropDown ??= new DropDownMenuBase(_pageLocator.Locator("div[data-testid='autocomplete-results']"), "li[role='option']");
        public ButtonBase SearchButton => _searchButton ??= new ButtonBase(_pageLocator.Locator("button[data-testid='searchbox-submit-button']"));
        public ButtonBase DatesButton => _datesButton ??= new ButtonBase(_pageLocator.Locator("button[data-testid='searchbox-dates-container']"));
        public ButtonBase OccupancyButton => _occupancyButton ??= new ButtonBase(_pageLocator.Locator("button[data-testid='occupancy-config']"));

        public SelectDatesPanel SelectDatesPanel => _selectDatesPanel ??= new SelectDatesPanel(_pageLocator, DatesButton);
        public OccupancyPanel OccupancyDropDown => _occupancyDropDown ??= new OccupancyPanel(_pageLocator, OccupancyButton);
        public CheckBoxBase TravelingForWorkCheckBox => _travelingForWorkCheckBox ??= new CheckBoxBase(_pageLocator.Locator("input[name='sb_travel_purpose']"));
        public CheckBoxBase AddFlightsToMySearchCheckBox => _addFlightsToMySearchCheckBox ??= new CheckBoxBase(_pageLocator.Locator("input[name='sb_flight_search']"));
    }
}
