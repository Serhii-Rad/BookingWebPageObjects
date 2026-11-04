using BookingWebPageObjects.Pages;
using NUnit.Framework;
using PlaywrightBaseFramework.Helpers.Extensions;
using System.Globalization;
using TestFrameworkCore.Asserts;
using TestFrameworkCore.Helpers;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.TestSuites
{
    [TestFixture]
    internal class FlightsSmokeTests : BookingSuiteBase
    {
        [Test]
        public async Task TC0011_TestFlightsSearchBoxElementsArePresent()
        {
            Step = 1;
            Assert.That(await FlightsPage.RoundTripRadioButton.IsVisibleAsync(), Is.True, "Round-trip radio button is not visible");
            Assert.That(await FlightsPage.OneWayRadioButton.IsVisibleAsync(), Is.True, "One-way radio button is not visible");
            Assert.That(await FlightsPage.MultiCityRadioButton.IsVisibleAsync(), Is.True, "Multi-city radio button is not visible");
            Assert.That(await FlightsPage.CabinClassDropDown.IsVisibleAsync(), Is.True, "Cabin class drop down is not visible");
            Assert.That(await FlightsPage.DirectFlightsOnlyCheckBox.IsVisibleAsync(), Is.True, "Direct flights only check box is not visible");

            Step = 2;
            Assert.That(await FlightsPage.FromButton.IsVisibleAsync(), Is.True, "From button is not visible");
            Assert.That(await FlightsPage.ToButton.IsVisibleAsync(), Is.True, "To button is not visible");
            Assert.That(await FlightsPage.SwapLocationsButton.IsVisibleAsync(), Is.True, "Swap locations button is not visible");
            Assert.That(await FlightsPage.DatesButton.IsVisibleAsync(), Is.True, "Dates button is not visible");
            Assert.That(await FlightsPage.TravelersButton.IsVisibleAsync(), Is.True, "Travelers button is not visible");
            Assert.That(await FlightsPage.SearchButton.IsVisibleAsync(), Is.True, "Search button is not visible");

            Step = 3;
            Assert.That(await FlightsPage.RoundTripRadioButton.IsCheckedAsync(), Is.True, "Round-trip is not selected by default");
            Assert.That(await FlightsPage.DirectFlightsOnlyCheckBox.IsCheckedAsync(), Is.False, "Direct flights only is checked by default");
            Assert.That(await FlightsPage.CabinClassDropDown.GetCurrentValueAsync(), Is.EqualTo("Economy"), "Default cabin class is incorrect");
            Assert.That(await FlightsPage.TravelersButton.GetTextAsync(), Does.Contain("1 adult"), "Default travelers amount is incorrect");
        }

        [Test]
        public async Task TC0012_TestTripTypeSwitching()
        {
            Step = 1;
            await FlightsPage.OneWayRadioButton.SetCheckAsync();
            TestAsserts.AssertIsTrue(await FlightsPage.OneWayRadioButton.IsCheckedAsync(), "One-way is not selected");
            TestAsserts.AssertIsFalse(await FlightsPage.RoundTripRadioButton.IsCheckedAsync(), "Round-trip is still selected after One-way selection");
            TestAsserts.AssertThat(await FlightsPage.DatesButton.GetTextAsync(), Does.Not.Contain("–"), "Dates button shows a return date for One-way trip");

            Step = 2;
            await FlightsPage.RoundTripRadioButton.SetCheckAsync();
            TestAsserts.AssertIsTrue(await FlightsPage.RoundTripRadioButton.IsCheckedAsync(), "Round-trip is not selected");
            TestAsserts.AssertIsFalse(await FlightsPage.OneWayRadioButton.IsCheckedAsync(), "One-way is still selected after Round-trip selection");
            TestAsserts.AssertThat(await FlightsPage.DatesButton.GetTextAsync(), Does.Contain("–"), "Dates button does not show a return date for Round-trip");
        }

        [Test]
        public async Task TC0013_TestCabinClassDropDown()
        {
            Step = 1;
            List<string> expectedCabinClasses = new List<string> { "Economy", "Premium economy", "Business", "First-class" };
            List<string?> actualCabinClasses = await FlightsPage.CabinClassDropDown.GetItemsListAsync();
            TestAsserts.AssertCollectionAreEqual(expectedCabinClasses, actualCabinClasses, "Cabin class options are incorrect");

            Step = 2;
            string cabinClassToSelect = "Business";
            await FlightsPage.CabinClassDropDown.SelectItemAsync(cabinClassToSelect);
            TestAsserts.AssertAreEqual(cabinClassToSelect, await FlightsPage.CabinClassDropDown.GetCurrentValueAsync(), "Selected cabin class is incorrect");
        }

        [Test]
        public async Task TC0014_TestDirectFlightsOnlyCheckBox()
        {
            Step = 1;
            await FlightsPage.DirectFlightsOnlyCheckBox.SetCheckAsync(true);
            TestAsserts.AssertIsTrue(await FlightsPage.DirectFlightsOnlyCheckBox.IsCheckedAsync(), "Direct flights only check box is not checked");

            Step = 2;
            await FlightsPage.DirectFlightsOnlyCheckBox.SetCheckAsync(false);
            TestAsserts.AssertIsFalse(await FlightsPage.DirectFlightsOnlyCheckBox.IsCheckedAsync(), "Direct flights only check box is still checked");
        }

        [Test]
        public async Task TC0015_TestSelectOriginAndDestination()
        {
            Step = 1;
            await FlightsPage.FromLocationPanel.SelectLocationAsync("Warsaw");
            TestAsserts.AssertThat(await FlightsPage.FromButton.GetTextAsync(), Does.Contain("Warsaw"), "Selected origin is not displayed on the From button");

            Step = 2;
            await FlightsPage.ToLocationPanel.SelectLocationAsync("London", "Heathrow");
            TestAsserts.AssertThat(await FlightsPage.ToButton.GetTextAsync(), Does.Contain("London Heathrow"), "Selected destination is not displayed on the To button");
        }

        [Test]
        public async Task TC0016_TestSwapOriginAndDestination()
        {
            Step = 1;
            await FlightsPage.FromLocationPanel.SelectLocationAsync("Warsaw");
            await FlightsPage.ToLocationPanel.SelectLocationAsync("London", "Heathrow");

            Step = 2;
            await FlightsPage.SwapLocationsButton.ClickAsync();
            TestAsserts.AssertThat(await FlightsPage.FromButton.GetTextAsync(), Does.Contain("London Heathrow"), "From button does not show the former destination after swap");
            TestAsserts.AssertThat(await FlightsPage.ToButton.GetTextAsync(), Does.Contain("Warsaw"), "To button does not show the former origin after swap");
        }

        [Test]
        public async Task TC0017_TestSelectTravelDates()
        {
            Step = 1;
            DateTime departureDate = DateTime.Today.AddDays(30);
            DateTime returnDate = departureDate.AddDays(5);
            await FlightsPage.DatesPanel.SelectDatesAsync(departureDate, returnDate);
            TestAsserts.AssertIsTrue(await FlightsPage.DatesPanel.IsHiddenAsync(), "Dates panel is not closed after selecting both dates");

            Step = 2;
            string? datesButtonText = await FlightsPage.DatesButton.GetTextAsync();
            TestAsserts.AssertThat(datesButtonText, Does.Contain(departureDate.ToString("ddd, MMM d", CultureInfo.InvariantCulture)), "Departure date is not displayed on the Dates button");
            TestAsserts.AssertThat(datesButtonText, Does.Contain(returnDate.ToString("ddd, MMM d", CultureInfo.InvariantCulture)), "Return date is not displayed on the Dates button");
        }

        [Test]
        public async Task TC0018_TestTravelersPanel()
        {
            Step = 1;
            await FlightsPage.TravelersPanel.OpenAsync();
            TestAsserts.AssertIsTrue(await FlightsPage.TravelersPanel.IsVisibleAsync(), "Travelers panel is not visible");

            Step = 2;
            string adults = "2";
            string children = "1";
            int childAge = 7;
            await FlightsPage.TravelersPanel.AdultsInputs.SetTextAsync(adults);
            await FlightsPage.TravelersPanel.ChildrenInputs.SetTextAsync(children);
            await FlightsPage.TravelersPanel.SelectChildAgeAsync(childAge);
            await FlightsPage.TravelersPanel.DoneButton.ClickAsync();
            TestAsserts.AssertThat(await FlightsPage.TravelersButton.GetTextAsync(), Does.Contain("3 travelers"), "Travelers amount on the Travelers button is incorrect");

            Step = 3;
            await FlightsPage.TravelersPanel.OpenAsync();
            TestAsserts.AssertAreEqual(adults, await FlightsPage.TravelersPanel.AdultsInputs.GetValueAsync(), "Adults amount is incorrect");
            TestAsserts.AssertAreEqual(children, await FlightsPage.TravelersPanel.ChildrenInputs.GetValueAsync(), "Children amount is incorrect");
            TestAsserts.AssertAreEqual(childAge.ToString(), await FlightsPage.TravelersPanel.GetChildAgeAsync(), "Child age is incorrect");
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
            TestAsserts.AssertThat(Page.Url, Does.Contain("from=WAW.CITY"), "Search URL has incorrect origin");
            TestAsserts.AssertThat(Page.Url, Does.Contain("to=LHR.AIRPORT"), "Search URL has incorrect destination");
            TestAsserts.AssertThat(Page.Url, Does.Contain($"depart={departureDate:yyyy-MM-dd}"), "Search URL has incorrect departure date");
            TestAsserts.AssertThat(Page.Url, Does.Contain($"return={returnDate:yyyy-MM-dd}"), "Search URL has incorrect return date");
            TestAsserts.AssertThat(Page.Url, Does.Contain("adults=2"), "Search URL has incorrect adults amount");
        }
    }
}
