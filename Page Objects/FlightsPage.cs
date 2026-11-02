using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Sub_panels;
using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Pages
{
    /// <summary>
    /// https://www.booking.com/flights/ - search box. Flights uses 'data-ui-name' attributes instead of 'data-testid' (Stays).
    /// Search submit redirects to a different host: https://flights.booking.com/flights/...
    /// </summary>
    internal class FlightsPage : PageBase
    {
        public FlightsPage(IPage page) : base(page)
        {
        }

        public CheckBox RoundTripRadioButton => new CheckBox(_pageLocator.Locator("input[data-ui-name='input_search_type_roundtrip']"));
        public CheckBox OneWayRadioButton => new CheckBox(_pageLocator.Locator("input[data-ui-name='input_search_type_oneway']"));
        public CheckBox MultiCityRadioButton => new CheckBox(_pageLocator.Locator("input[data-ui-name='input_search_type_multistop']"));
        public DropDownMenu CabinClassDropDown => new DropDownMenu(_pageLocator.Locator("select[data-ui-name='cabin_class_input']"), "option");
        public CheckBox DirectFlightsOnlyCheckBox => new CheckBox(_pageLocator.Locator("input[data-ui-name='direct_flights_input']"));

        public Button FromButton => new Button(_pageLocator.Locator("button[data-ui-name='input_location_from_segment_0']"));
        public Button ToButton => new Button(_pageLocator.Locator("button[data-ui-name='input_location_to_segment_0']"));
        public Button SwapLocationsButton => new Button(_pageLocator.Locator("button[aria-label^='Switch origin']"));
        public Button DatesButton => new Button(_pageLocator.Locator("button[data-ui-name='button_date_segment_0']"));
        public Button TravelersButton => new Button(_pageLocator.Locator("button[data-ui-name='button_occupancy']"));
        public Button SearchButton => new Button(_pageLocator.Locator("button[data-ui-name='button_search_submit']"));

        public FlightsLocationPanel FromLocationPanel => new FlightsLocationPanel(_pageLocator, FromButton);
        public FlightsLocationPanel ToLocationPanel => new FlightsLocationPanel(_pageLocator, ToButton);
        public FlightsDatesPanel DatesPanel => new FlightsDatesPanel(_pageLocator, DatesButton);
        public FlightsTravelersPanel TravelersPanel => new FlightsTravelersPanel(_pageLocator, TravelersButton);
    }
}
