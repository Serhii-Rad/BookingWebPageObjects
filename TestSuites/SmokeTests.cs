using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Page_Objects;
using NUnit.Framework;
using TestFrameworkCore.Asserts;

namespace BookingWebPageObjects.TestSuites
{
    [TestFixture]
    internal class SmokeTests : BookingSuiteBase
    {
        [Test]
        public async Task TC0001_TestAllExpectedTabsButtonsArePresent()
        {
            Step = 1;
            List<string> expectedTabsNames = new List<string> { "Stays", "Flights", "Flight + Hotel", "Car rental" };
            IReadOnlyList<Button> actualButtonsList = await PageMain.TabsMenu.GetMenuElementsAsync(locator => new Button(locator));
            List<string> actualButtonsText = new List<string>();
            foreach (Button button in actualButtonsList)
            {
                string? buttonText = await button.GetTextAsync();
                actualButtonsText.Add(buttonText);
            }
            TestAsserts.AssertAreEqual(6, actualButtonsList.Count, "There are incorrect main tabs items amount");
            for (int i = 0; i < 4; i++)
            {
                TestAsserts.AssertAreEqual(expectedTabsNames[i], actualButtonsText[i], $"The tab at index {i} is incorrect");
            }
        }

        [Test]
        public async Task TC0002_TestCurrencyButtonFunctionality()
        {
            Step = 1;
            TestAsserts.AssertIsTrue(await PageMain.CurrencyButton.IsVisibleAsync(), "Currency button is not visible");

            Step = 2;
            string? currentCurrency = await PageMain.CurrencyButton.GetTextAsync();
            TestAsserts.AssertIsTrue(!string.IsNullOrEmpty(currentCurrency), "Current currency is not displayed on the button");

            Step = 3;
            await PageMain.CurrencyButton.ClickAsync();
            TestAsserts.AssertIsTrue(await PageMain.CurrencyPanel.IsVisibleAsync(), "Currency panel is not visible");
        }

        [Test]
        public async Task TC0003_TestCurrencyPanelFunctionality()
        {
            Step = 1;
            string currencyCodeToSelect = "USD";
            await PageMain.CurrencyPanel.OpenAsync();
            TestAsserts.AssertIsTrue(await PageMain.CurrencyPanel.IsVisibleAsync(), "Currency panel is not visible");
            if (await PageMain.CurrencyPanel.GetSelectedCurrency() == currencyCodeToSelect)
                currencyCodeToSelect = "EUR";
            await PageMain.CurrencyPanel.SelectCurrency(currencyCodeToSelect);

            Step = 2;
            TestAsserts.AssertAreEqual(currencyCodeToSelect, await PageMain.CurrencyButton.GetTextAsync(), "The currency button displays an incorrect currency");

            Step = 3;
            await Page.WaitForTimeoutAsync(1500);
            string selectedCurrency = await PageMain.CurrencyPanel.GetSelectedCurrency();
            TestAsserts.AssertThat(selectedCurrency, Does.Contain(currencyCodeToSelect), "The selected currency is incorrect");
        }

        [Test]
        public async Task TC0004_TestLanguageButtonFunctionality()
        {
            Step = 1;
            TestAsserts.AssertIsTrue(await PageMain.LanguageButton.IsVisibleAsync(), "Language button is not visible");

            Step = 2;
            string? currentLanguage = await PageMain.LanguageButton.GetTextAsync();
            TestAsserts.AssertIsTrue(!string.IsNullOrEmpty(currentLanguage), "Current language is not displayed on the button");

            Step = 3;
            await PageMain.LanguageButton.ClickAsync();
            TestAsserts.AssertIsTrue(await PageMain.LanguagePanel.IsVisibleAsync(), "Language panel is not visible");
        }

        [Test]
        public async Task TC0005_TestLanguagePanelFunctionality()
        {
            Step = 1;
            string languageToSelect = "English (US)";
            await PageMain.LanguagePanel.OpenAsync();
            TestAsserts.AssertIsTrue(await PageMain.LanguagePanel.IsVisibleAsync(), "Language panel is not visible");
            if (await PageMain.LanguagePanel.GetSelectedLanguage() == languageToSelect)
                languageToSelect = "Español";
            await PageMain.LanguagePanel.SelectLanguage(languageToSelect);

            Step = 2;
            TestAsserts.AssertAreEqual(languageToSelect, await PageMain.LanguageButton.GetTextAsync(), "The language button displays an incorrect language");

            Step = 3;
            await Page.WaitForTimeoutAsync(1500);
            string selectedLanguage = await PageMain.LanguagePanel.GetSelectedLanguage();
            TestAsserts.AssertThat(selectedLanguage, Does.Contain(languageToSelect), "The selected language is incorrect");
        }

        [Test]
        public async Task TC0006_TestCustomerServiceButtonFunctionality()
        {
            Step = 1;
            TestAsserts.AssertIsTrue(await PageMain.CustomerServiceButton.IsVisibleAsync(), "Customer Service button is not visible");

            Step = 2;
            await PageMain.CustomerServiceButton.ClickOnLinkAsync();
            TestAsserts.AssertThat(Page.Url, Does.Contain("https://secure.booking.com/"), "The Customer Service button did not navigate to the expected URL");
        }

        [Test]
        public async Task TC0007_TestListYourPropertyButtonFunctionality()
        {
            Step = 1;
            TestAsserts.AssertIsTrue(await PageMain.ListYourPropertyButton.IsVisibleAsync(), "List Your Property button is not visible");
            Step = 2;
            await PageMain.ListYourPropertyButton.ClickOnLinkAsync();
            TestAsserts.AssertThat(Page.Url, Does.Contain("https://join.booking.com/"), "The List Your Property button did not navigate to the expected URL");
        }

        [Test]
        public async Task TC0008_TestOccupancy()
        {
            Step = 1;
            TestAsserts.AssertIsTrue(await PageMain.UserProfileButton.IsVisibleAsync(), "User Profile button is not visible");
            Step = 2;
            await PageMain.UserProfileButton.ClickAsync();
            TestAsserts.AssertIsTrue(await PageMain.UserDropDownMenu.IsVisibleAsync(), "User Profile dropdown is not visible");
        }

        [Test]
        public async Task TC0009_TestCheckInOutDates()
        {
            Step = 1;
            TestAsserts.AssertIsTrue(await StaysPage.DatesButton.IsVisibleAsync(), "Check-in date picker is not visible");

            Step = 2;
            await StaysPage.SelectDatesPanel.OpenAsync();
            TestAsserts.AssertIsTrue(await StaysPage.SelectDatesPanel.IsVisibleAsync(), "Check-in date picker panel is not visible");

            Step = 3;
            DateTime checkInDate = DateTime.Today.AddDays(7);
            DateTime checkOutDate = DateTime.Today.AddDays(14);
            await StaysPage.SelectDatesPanel.SelectDates(checkInDate, checkOutDate);
            TestAsserts.AssertAreEqual($"Select dates{checkInDate.ToString("ddd, MMM d")} — {checkOutDate.ToString("ddd, MMM d")}", await StaysPage.DatesButton.GetTextAsync(), "The selected check-in date is incorrect");
            TestAsserts.AssertAreEqual(checkInDate, (await StaysPage.SelectDatesPanel.GetSelectedDatesAsync()).checkInDate, "The selected check-in date is incorrect");
            TestAsserts.AssertAreEqual(checkOutDate, (await StaysPage.SelectDatesPanel.GetSelectedDatesAsync()).checkOutDate, "The selected check-out date is incorrect");
        }

        [Test]
        public async Task TC0010_TestOccupancy()
        {
            Step = 1;
            TestAsserts.AssertIsTrue(await StaysPage.OccupancyButton.IsVisibleAsync(), "Occupancy button is not visible");

            Step = 2;
            await StaysPage.OccupancyDropDown.OpenAsync();
            TestAsserts.AssertIsTrue(await StaysPage.OccupancyDropDown.IsVisibleAsync());

            Step = 3;
            string adults = "5";
            string children = "1";
            string rooms = "3";
            await StaysPage.OccupancyDropDown.AdultsInputs.SetTextAsync(adults);
            await StaysPage.OccupancyDropDown.ChildrenInputs.SetTextAsync(children);
            await StaysPage.OccupancyDropDown.GetChildAgeDropDown().SelectItemAsync("0 years old");
            await StaysPage.OccupancyDropDown.RoomsInputs.SetTextAsync(rooms);
            await StaysPage.OccupancyDropDown.TravelingWithPetsCheckBox.SetCheckAsync(true);
            await StaysPage.OccupancyDropDown.DoneButton.ClickAsync();

            TestAsserts.AssertAreEqual($"{adults} adults · {children} children · Pets · {rooms} rooms", await StaysPage.OccupancyButton.GetTextAsync());

            Step = 4;
            await StaysPage.OccupancyDropDown.OpenAsync();

            TestAsserts.AssertAreEqual(adults, await StaysPage.OccupancyDropDown.AdultsInputs.GetValueAsync());
            TestAsserts.AssertAreEqual(children, await StaysPage.OccupancyDropDown.AdultsInputs.GetValueAsync());
            TestAsserts.AssertAreEqual("0 years old", await StaysPage.OccupancyDropDown.GetChildAgeDropDown().GetCurrentValueAsync());
            TestAsserts.AssertAreEqual(rooms, await StaysPage.OccupancyDropDown.AdultsInputs.GetValueAsync());
            TestAsserts.AssertIsTrue(await StaysPage.OccupancyDropDown.TravelingWithPetsCheckBox.IsCheckedAsync());

        }

        //[Test]
        //public async Task TestUserDropDownMenuFunctionality()
        //{
        //    Step = 1;
        //    await PageMain.UserDropDownMenu.OpenDropDownAsync();
        //    List<string> expectedMenuItems = new List<string> { "Sign in", "Register", "My bookings", "My profile", "Account", "Help" };
        //    List<string?> actualMenuItems = await PageMain.UserDropDownMenu.GetItemsListAsync();
        //    TestAsserts.AssertAreEqual(expectedMenuItems.Count, actualMenuItems.Count, "The number of items in the User Profile dropdown is incorrect");
        //    for (int i = 0; i < expectedMenuItems.Count; i++)
        //    {
        //        TestAsserts.AssertAreEqual(expectedMenuItems[i], actualMenuItems[i], $"The menu item at index {i} is incorrect");
        //    }
        //}
    }
}
