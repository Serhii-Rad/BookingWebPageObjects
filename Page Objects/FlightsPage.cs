using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Sub_panels;
using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Pages
{
    internal class FlightsPage : PageBase
    {
        private static readonly string _pageSelector = "main";

        private RadioButton? _roundTripRadioButton;
        private RadioButton? _oneWayRadioButton;
        private RadioButton? _multiCityRadioButton;
        private DropDownMenu? _cabinClassDropDown;
        private CheckBox? _directFlightsOnlyCheckBox;
        private Button? _fromButton;
        private Button? _toButton;  
        private Button? _swapLocationsButton;
        private Button? _datesButton;
        private Button? _travelersButton;
        private Button? _searchButton;
        private FlightsLocationPanel? _fromLocationPanel;
        private FlightsLocationPanel? _toLocationPanel;
        private FlightsDatesPanel? _datesPanel;
        private FlightsTravelersPanel? _travelersPanel;

        public FlightsPage(IPage page) : base(page.Locator(_pageSelector))
        {
        }

        public RadioButton RoundTripRadioButton => _roundTripRadioButton ??= new RadioButton(_pageLocator.Locator("input[data-ui-name='input_search_type_roundtrip']"));
        public RadioButton OneWayRadioButton => _oneWayRadioButton ??= new RadioButton(_pageLocator.Locator("input[data-ui-name='input_search_type_oneway']"));
        public RadioButton MultiCityRadioButton => _multiCityRadioButton ??= new RadioButton(_pageLocator.Locator("input[data-ui-name='input_search_type_multistop']"));
        public DropDownMenu CabinClassDropDown => _cabinClassDropDown ??= new DropDownMenu(_pageLocator.Locator("select[data-ui-name='cabin_class_input']"), "option");
        public CheckBox DirectFlightsOnlyCheckBox => _directFlightsOnlyCheckBox ??= new CheckBox(_pageLocator.Locator("input[data-ui-name='direct_flights_input']"));

        public Button FromButton => _fromButton ??= new Button(_pageLocator.Locator("button[data-ui-name='input_location_from_segment_0']"));
        public Button ToButton => _toButton ??= new Button(_pageLocator.Locator("button[data-ui-name='input_location_to_segment_0']"));
        public Button SwapLocationsButton => _swapLocationsButton ??= new Button(_pageLocator.Locator("button[aria-label^='Switch origin']"));
        public Button DatesButton => _datesButton ??= new Button(_pageLocator.Locator("button[data-ui-name='button_date_segment_0']"));
        public Button TravelersButton => _travelersButton ??= new Button(_pageLocator.Locator("button[data-ui-name='button_occupancy']"));
        public Button SearchButton => _searchButton ??= new Button(_pageLocator.Locator("button[data-ui-name='button_search_submit']"));

        public FlightsLocationPanel FromLocationPanel => _fromLocationPanel ??= new FlightsLocationPanel(_pageLocator, FromButton);
        public FlightsLocationPanel ToLocationPanel => _toLocationPanel ??= new FlightsLocationPanel(_pageLocator, ToButton);
        public FlightsDatesPanel DatesPanel => _datesPanel ??= new FlightsDatesPanel(_pageLocator, DatesButton);
        public FlightsTravelersPanel TravelersPanel => _travelersPanel ??= new FlightsTravelersPanel(_pageLocator, TravelersButton);
    }
}
