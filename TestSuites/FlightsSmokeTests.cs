using BookingWebPageObjects.Pages;
using NUnit.Framework;
using System.Globalization;

namespace BookingWebPageObjects.TestSuites
{
    [TestFixture]
    internal class FlightsSmokeTests : BookingSuiteBase
    {
        [Test]
        public async Task TC0011_TestFlightsSearchBoxElementsArePresent()
        {
            Step = 1;
            using (Assert.EnterMultipleScope())
            {
                Assert.That(await FlightsPage.RoundTripRadioButton.IsVisibleAsync(), Is.True, "Round-trip radio button is not visible");
                Assert.That(await FlightsPage.OneWayRadioButton.IsVisibleAsync(), Is.True, "One-way radio button is not visible");
                Assert.That(await FlightsPage.MultiCityRadioButton.IsVisibleAsync(), Is.True, "Multi-city radio button is not visible");
                Assert.That(await FlightsPage.CabinClassDropDown.IsVisibleAsync(), Is.True, "Cabin class drop down is not visible");
                Assert.That(await FlightsPage.DirectFlightsOnlyCheckBox.IsVisibleAsync(), Is.True, "Direct flights only check box is not visible");

                Assert.That(await FlightsPage.FromButton.IsVisibleAsync(), Is.True, "From button is not visible");
                Assert.That(await FlightsPage.ToButton.IsVisibleAsync(), Is.True, "To button is not visible");
                Assert.That(await FlightsPage.SwapLocationsButton.IsVisibleAsync(), Is.True, "Swap locations button is not visible");
                Assert.That(await FlightsPage.DatesButton.IsVisibleAsync(), Is.True, "Dates button is not visible");
                Assert.That(await FlightsPage.TravelersButton.IsVisibleAsync(), Is.True, "Travelers button is not visible");
                Assert.That(await FlightsPage.SearchButton.IsVisibleAsync(), Is.True, "Search button is not visible");

                Assert.That(await FlightsPage.RoundTripRadioButton.IsCheckedAsync(), Is.True, "Round-trip is not selected by default");
                Assert.That(await FlightsPage.DirectFlightsOnlyCheckBox.IsCheckedAsync(), Is.False, "Direct flights only is checked by default");
                Assert.That(await FlightsPage.CabinClassDropDown.GetCurrentValueAsync(), Is.EqualTo("Economy"), "Default cabin class is incorrect");
                Assert.That(await FlightsPage.TravelersButton.GetTextAsync(), Does.Contain("1 adult"), "Default travelers amount is incorrect");

            }
        }

        [Test]
        public async Task TC0012_TestTripTypeSwitching()
        {
            Step = 1;
            await FlightsPage.OneWayRadioButton.SetCheckAsync();
            using (Assert.EnterMultipleScope())
            {
                Assert.That(await FlightsPage.OneWayRadioButton.IsCheckedAsync(), Is.True, "One-way is not selected");
                Assert.That(await FlightsPage.RoundTripRadioButton.IsCheckedAsync(), Is.False, "Round-trip is still selected after One-way selection");
                Assert.That(await FlightsPage.DatesButton.GetTextAsync(), Does.Not.Contain("–"), "Dates button shows a return date for One-way trip");
            }

            Step = 2;
            await FlightsPage.RoundTripRadioButton.SetCheckAsync();
            using (Assert.EnterMultipleScope())
            {
                Assert.That(await FlightsPage.RoundTripRadioButton.IsCheckedAsync(), Is.True, "Round-trip is not selected");
                Assert.That(await FlightsPage.OneWayRadioButton.IsCheckedAsync(), Is.False, "One-way is still selected after Round-trip selection");
                Assert.That(await FlightsPage.DatesButton.GetTextAsync(), Does.Contain("–"), "Dates button does not show a return date for Round-trip");
            }
        }

        [Test]
        public async Task TC0013_TestCabinClassDropDown()
        {
            Step = 1;
            List<string> expectedCabinClasses = new List<string> { "Economy", "Premium economy", "Business", "First-class" };
            List<string> actualCabinClasses = await FlightsPage.CabinClassDropDown.GetItemsListAsync();
            Assert.That(actualCabinClasses, Is.EquivalentTo(expectedCabinClasses), "Cabin class options are incorrect");

            Step = 2;
            string cabinClassToSelect = "Business";
            await FlightsPage.CabinClassDropDown.SelectItemAsync(cabinClassToSelect);
            Assert.That(await FlightsPage.CabinClassDropDown.GetCurrentValueAsync(), Is.EqualTo(cabinClassToSelect), "Selected cabin class is incorrect");
        }

        [Test]
        public async Task TC0014_TestDirectFlightsOnlyCheckBox()
        {
            Step = 1;
            await FlightsPage.DirectFlightsOnlyCheckBox.SetCheckAsync(true);
            Assert.That(await FlightsPage.DirectFlightsOnlyCheckBox.IsCheckedAsync(), Is.True, "Direct flights only check box is not checked");

            Step = 2;
            await FlightsPage.DirectFlightsOnlyCheckBox.SetCheckAsync(false);
            Assert.That(await FlightsPage.DirectFlightsOnlyCheckBox.IsCheckedAsync(), Is.False, "Direct flights only check box is still checked");
        }

        [Test]
        public async Task TC0015_TestSelectOriginAndDestination()
        {
            Step = 1;
            await FlightsPage.FromLocationPanel.SelectLocationAsync("Warsaw");
            Assert.That(await FlightsPage.FromButton.GetTextAsync(), Does.Contain("Warsaw"), "Selected origin is not displayed on the From button");

            Step = 2;
            await FlightsPage.ToLocationPanel.SelectLocationAsync("London", "Heathrow");
            Assert.That(await FlightsPage.ToButton.GetTextAsync(), Does.Contain("London Heathrow"), "Selected destination is not displayed on the To button");
        }

        [Test]
        public async Task TC0016_TestSwapOriginAndDestination()
        {
            Step = 1;
            await FlightsPage.FromLocationPanel.SelectLocationAsync("Warsaw");
            await FlightsPage.ToLocationPanel.SelectLocationAsync("London", "Heathrow");

            Step = 2;
            await FlightsPage.SwapLocationsButton.ClickAsync();
            using (Assert.EnterMultipleScope())
            {
                Assert.That(await FlightsPage.FromButton.GetTextAsync(), Does.Contain("London Heathrow"), "From button does not show the former destination after swap");
                Assert.That(await FlightsPage.ToButton.GetTextAsync(), Does.Contain("Warsaw"), "To button does not show the former origin after swap");
            }
        }

        [Test]
        public async Task TC0017_TestSelectTravelDates()
        {
            Step = 1;
            DateTime departureDate = DateTime.Today.AddDays(30);
            DateTime returnDate = departureDate.AddDays(5);
            await FlightsPage.DatesPanel.SelectDatesAsync(departureDate, returnDate);
            Assert.That(await FlightsPage.DatesPanel.IsHiddenAsync(), Is.True, "Dates panel is not closed after selecting both dates");

            Step = 2;
            string? datesButtonText = await FlightsPage.DatesButton.GetTextAsync();
            using (Assert.EnterMultipleScope())
            {
                Assert.That(datesButtonText, Does.Contain(departureDate.ToString("ddd, MMM d", CultureInfo.InvariantCulture)), "Departure date is not displayed on the Dates button");
                Assert.That(datesButtonText, Does.Contain(returnDate.ToString("ddd, MMM d", CultureInfo.InvariantCulture)), "Return date is not displayed on the Dates button");
            }
        }

        [Test]
        public async Task TC0018_TestTravelersPanel()
        {
            Step = 1;
            await FlightsPage.TravelersPanel.OpenAsync();
            Assert.That(await FlightsPage.TravelersPanel.IsVisibleAsync(), Is.True, "Travelers panel is not visible");

            Step = 2;
            string adults = "2";
            string children = "1";
            int childAge = 7;
            await FlightsPage.TravelersPanel.AdultsInputs.SetTextAsync(adults);
            await FlightsPage.TravelersPanel.ChildrenInputs.SetTextAsync(children);
            await FlightsPage.TravelersPanel.SelectChildAgeAsync(childAge);
            await FlightsPage.TravelersPanel.DoneButton.ClickAsync();
            Assert.That(await FlightsPage.TravelersButton.GetTextAsync(), Does.Contain("3 travelers"), "Travelers amount on the Travelers button is incorrect");

            Step = 3;
            await FlightsPage.TravelersPanel.OpenAsync();
            using (Assert.EnterMultipleScope())
            {
                Assert.That(await FlightsPage.TravelersPanel.AdultsInputs.GetValueAsync(), Is.EqualTo(adults), "Adults amount is incorrect");
                Assert.That(await FlightsPage.TravelersPanel.ChildrenInputs.GetValueAsync(), Is.EqualTo(children), "Children amount is incorrect");
                Assert.That(await FlightsPage.TravelersPanel.GetChildAgeAsync(), Is.EqualTo(childAge.ToString()), "Child age is incorrect");

            }
        }

        [Test]
        public async Task TC0019_TestSearchFlights()
        {
            Step = 1;
            DateTime departureDate = DateTime.Today.AddDays(30);
            DateTime returnDate = departureDate.AddDays(5);
            await FlightsPage.FromLocationPanel.SelectLocationAsync("Warsaw");
            await FlightsPage.ToLocationPanel.SelectLocationAsync("London", "Heathrow");
            await FlightsPage.DatesPanel.SelectDatesAsync(departureDate, returnDate);
            await FlightsPage.TravelersPanel.OpenAsync();
            await FlightsPage.TravelersPanel.AdultsInputs.SetTextAsync("2");
            await FlightsPage.TravelersPanel.DoneButton.ClickAsync();

            Step = 2;
            await FlightsPage.SearchButton.ClickAsync();
            await Page.WaitForURLAsync("**flights.booking.com/flights/**");
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Page.Url, Does.Contain("from=WAW.CITY"), "Search URL has incorrect origin");
                Assert.That(Page.Url, Does.Contain("to=LHR.AIRPORT"), "Search URL has incorrect destination");
                Assert.That(Page.Url, Does.Contain($"depart={departureDate:yyyy-MM-dd}"), "Search URL has incorrect departure date");
                Assert.That(Page.Url, Does.Contain($"return={returnDate:yyyy-MM-dd}"), "Search URL has incorrect return date");
                Assert.That(Page.Url, Does.Contain("adults=2"), "Search URL has incorrect adults amount");
            }
        }
    }
}
