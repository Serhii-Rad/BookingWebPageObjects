using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Sub_panels;
using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Pages
{
    internal class FlightsPage : PageBase
    {
        private static readonly string _pageSelector = "main";

        private RadioButtonBase? _roundTripRadioButton;
        private RadioButtonBase? _oneWayRadioButton;
        private RadioButtonBase? _multiCityRadioButton;
        private DropDownMenuBase? _cabinClassDropDown;
        private CheckBoxBase? _directFlightsOnlyCheckBox;
        private ButtonBase? _fromButton;
        private ButtonBase? _toButton;  
        private ButtonBase? _swapLocationsButton;
        private ButtonBase? _datesButton;
        private ButtonBase? _travelersButton;
        private ButtonBase? _searchButton;
        private FlightsLocationPanel? _fromLocationPanel;
        private FlightsLocationPanel? _toLocationPanel;
        private FlightsDatesPanel? _datesPanel;
        private FlightsTravelersPanel? _travelersPanel;

        public FlightsPage(IPage page) : base(page.Locator(_pageSelector))
        {
        }

        public RadioButtonBase RoundTripRadioButton => _roundTripRadioButton ??= new RadioButtonBase(_pageLocator.Locator("input[data-ui-name='input_search_type_roundtrip']"));
        public RadioButtonBase OneWayRadioButton => _oneWayRadioButton ??= new RadioButtonBase(_pageLocator.Locator("input[data-ui-name='input_search_type_oneway']"));
        public RadioButtonBase MultiCityRadioButton => _multiCityRadioButton ??= new RadioButtonBase(_pageLocator.Locator("input[data-ui-name='input_search_type_multistop']"));
        public DropDownMenuBase CabinClassDropDown => _cabinClassDropDown ??= new DropDownMenuBase(_pageLocator.Locator("select[data-ui-name='cabin_class_input']"), "option");
        public CheckBoxBase DirectFlightsOnlyCheckBox => _directFlightsOnlyCheckBox ??= new CheckBoxBase(_pageLocator.Locator("input[data-ui-name='direct_flights_input']"));

        public ButtonBase FromButton => _fromButton ??= new ButtonBase(_pageLocator.Locator("button[data-ui-name='input_location_from_segment_0']"));
        public ButtonBase ToButton => _toButton ??= new ButtonBase(_pageLocator.Locator("button[data-ui-name='input_location_to_segment_0']"));
        public ButtonBase SwapLocationsButton => _swapLocationsButton ??= new ButtonBase(_pageLocator.Locator("button[aria-label^='Switch origin']"));
        public ButtonBase DatesButton => _datesButton ??= new ButtonBase(_pageLocator.Locator("button[data-ui-name='button_date_segment_0']"));
        public ButtonBase TravelersButton => _travelersButton ??= new ButtonBase(_pageLocator.Locator("button[data-ui-name='button_occupancy']"));
        public ButtonBase SearchButton => _searchButton ??= new ButtonBase(_pageLocator.Locator("button[data-ui-name='button_search_submit']"));

        public FlightsLocationPanel FromLocationPanel => _fromLocationPanel ??= new FlightsLocationPanel(_pageLocator, FromButton);
        public FlightsLocationPanel ToLocationPanel => _toLocationPanel ??= new FlightsLocationPanel(_pageLocator, ToButton);
        public FlightsDatesPanel DatesPanel => _datesPanel ??= new FlightsDatesPanel(_pageLocator, DatesButton);
        public FlightsTravelersPanel TravelersPanel => _travelersPanel ??= new FlightsTravelersPanel(_pageLocator, TravelersButton);
    }
}
