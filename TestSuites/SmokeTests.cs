using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Page_Objects;
using NUnit.Framework;

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
            List<string?> actualButtonsText = new List<string?>();
            foreach (Button button in actualButtonsList)
            {
                string? buttonText = await button.GetTextAsync();
                actualButtonsText.Add(buttonText);
            }
            Assert.That(6, Is.EqualTo(actualButtonsList.Count), "There are incorrect main tabs items amount");
            for (int i = 0; i < 4; i++)
            {
                Assert.That(expectedTabsNames[i], Is.EqualTo(actualButtonsText[i]), $"The tab at index {i} is incorrect");
            }
        }

        [Test]
        public async Task TC0002_TestCurrencyButtonFunctionality()
        {
            Step = 1;
            Assert.That(await PageMain.CurrencyButton.IsVisibleAsync(), Is.True, "Currency button is not visible");

            Step = 2;
            string? currentCurrency = await PageMain.CurrencyButton.GetTextAsync();
            Assert.That(string.IsNullOrEmpty(currentCurrency), Is.False, "Current currency is not displayed on the button");

            Step = 3;
            await PageMain.CurrencyButton.ClickAsync();
            Assert.That(await PageMain.CurrencyPanel.IsVisibleAsync(), Is.True, "Currency panel is not visible");
        }

        [Test]
        public async Task TC0003_TestCurrencyPanelFunctionality()
        {
            Step = 1;
            string currencyCodeToSelect = "USD";
            await PageMain.CurrencyPanel.OpenAsync();
            Assert.That(await PageMain.CurrencyPanel.IsVisibleAsync(), Is.True, "Currency panel is not visible");
            if (await PageMain.CurrencyPanel.GetSelectedCurrency() == currencyCodeToSelect)
                currencyCodeToSelect = "EUR";
            await PageMain.CurrencyPanel.SelectCurrency(currencyCodeToSelect);

            Step = 2;
            Assert.That(currencyCodeToSelect, Is.EqualTo(await PageMain.CurrencyButton.GetTextAsync()), "The currency button displays an incorrect currency");

            Step = 3;
            await Page.WaitForTimeoutAsync(1500);
            string selectedCurrency = await PageMain.CurrencyPanel.GetSelectedCurrency();
            Assert.That(selectedCurrency, Does.Contain(currencyCodeToSelect), "The selected currency is incorrect");
        }

        [Test]
        public async Task TC0004_TestLanguageButtonFunctionality()
        {
            Step = 1;
            Assert.That(await PageMain.LanguageButton.IsVisibleAsync(), Is.True, "Language button is not visible");

            Step = 2;
            string? currentLanguage = await PageMain.LanguageButton.GetTextAsync();
            Assert.That(string.IsNullOrEmpty(currentLanguage), Is.True, "Current language is not displayed on the button");
            Step = 3;
            await PageMain.LanguageButton.ClickAsync();
            Assert.That(await PageMain.LanguagePanel.IsVisibleAsync(), Is.True, "Language panel is not visible");
        }

        [Test]
        public async Task TC0005_TestLanguagePanelFunctionality()
        {
            Step = 1;
            string languageToSelect = "English (US)";
            await PageMain.LanguagePanel.OpenAsync();
            Assert.That(await PageMain.LanguagePanel.IsVisibleAsync(), Is.True, "Language panel is not visible");
            if (await PageMain.LanguagePanel.GetSelectedLanguage() == languageToSelect)
                languageToSelect = "Español";
            await PageMain.LanguagePanel.SelectLanguage(languageToSelect);

            Step = 2;
            await Page.WaitForTimeoutAsync(1500);
            await PageMain.LanguagePanel.OpenAsync();
            string selectedLanguage = await PageMain.LanguagePanel.GetSelectedLanguage();
            Assert.That(selectedLanguage, Does.Contain(languageToSelect), "The selected language is incorrect");
        }

        [Test]
        public async Task TC0006_TestCustomerServiceButtonFunctionality()
        {
            Step = 1;
            Assert.That(await PageMain.CustomerServiceButton.IsVisibleAsync(), Is.True, "Customer Service button is not visible");

            Step = 2;
            await PageMain.CustomerServiceButton.ClickAsync();
            Assert.That(Page.Url, Does.Contain("https://secure.booking.com/"), "The Customer Service button did not navigate to the expected URL");
        }

        [Test]
        public async Task TC0007_TestListYourPropertyButtonFunctionality()
        {
            Step = 1;
            Assert.That(await PageMain.ListYourPropertyButton.IsVisibleAsync(), Is.True, "List Your Property button is not visible");
            Step = 2;
            await PageMain.ListYourPropertyButton.ClickAsync();
            Assert.That(Page.Url, Does.Contain("https://join.booking.com/"), "The List Your Property button did not navigate to the expected URL");
        }

        [Test]
        public async Task TC0008_TestOccupancy()
        {
            Step = 1;
            Assert.That(await PageMain.UserProfileButton.IsVisibleAsync(), Is.True, "User Profile button is not visible"); // fails because there is no such button if you are not logged in
            Step = 2;
            await PageMain.UserProfileButton.ClickAsync();
            Assert.That(await PageMain.UserDropDownMenu.IsVisibleAsync(), Is.True, "User Profile dropdown is not visible");
        }

        [Test]
        public async Task TC0009_TestCheckInOutDates()
        {
            Step = 1;
            Assert.That(await StaysPage.DatesButton.IsVisibleAsync(), Is.True, "Check-in date picker is not visible");

            Step = 2;
            await StaysPage.SelectDatesPanel.OpenAsync();
            Assert.That(await StaysPage.SelectDatesPanel.IsVisibleAsync(), Is.True, "Check-in date picker panel is not visible");

            Step = 3;
            DateTime checkInDate = DateTime.Today.AddDays(7);
            DateTime checkOutDate = DateTime.Today.AddDays(14);
            await StaysPage.SelectDatesPanel.SelectDates(checkInDate, checkOutDate);
            using (Assert.EnterMultipleScope())
            {
                Assert.That($"Select dates{checkInDate.ToString("ddd, MMM d")} — {checkOutDate.ToString("ddd, MMM d")}", Is.EqualTo(await StaysPage.DatesButton.GetTextAsync()), "The selected check-in date is incorrect");
                Assert.That(checkInDate, Is.EqualTo((await StaysPage.SelectDatesPanel.GetSelectedDatesAsync()).checkInDate), "The selected check-in date is incorrect");
                Assert.That(checkOutDate, Is.EqualTo((await StaysPage.SelectDatesPanel.GetSelectedDatesAsync()).checkOutDate), "The selected check-out date is incorrect");
            }
        }

        [Test]
        public async Task TC0010_TestOccupancy()
        {
            Step = 1;
            Assert.That(await StaysPage.OccupancyButton.IsVisibleAsync(), Is.True, "Occupancy button is not visible");
            string? currentValue = await StaysPage.OccupancyButton.GetTextAsync();
            Assert.That(currentValue, Is.EqualTo("2 adults · 0 children · 1 room"), "The occupancy button text is incorrect");

            Step = 2;
            await StaysPage.OccupancyDropDown.OpenAsync();
            Assert.That(await StaysPage.OccupancyDropDown.IsVisibleAsync(), Is.True, "Occupancy dropdown is not visible");

            Step = 3;
            string adults = "5";
            string children = "1";
            string rooms = "3";
            await StaysPage.OccupancyDropDown.OpenAsync();
            await StaysPage.OccupancyDropDown.AdultsInputs.SetTextAsync(adults);
            await StaysPage.OccupancyDropDown.ChildrenInputs.SetTextAsync(children);
            await StaysPage.OccupancyDropDown.GetChildAgeDropDown().SelectItemAsync("10", false);
            await StaysPage.OccupancyDropDown.RoomsInputs.SetTextAsync(rooms);
            await StaysPage.OccupancyDropDown.TravelingWithPetsCheckBox.SetCheckAsync(true);
            await StaysPage.OccupancyDropDown.DoneButton.ClickAsync();
            currentValue = await StaysPage.OccupancyButton.GetTextAsync();
            Assert.That(currentValue, Is.EqualTo($"{adults} adults · {children} child · Pets · {rooms} rooms"), "The occupancy button text is incorrect");

            Step = 4;
            await StaysPage.OccupancyDropDown.OpenAsync();
            using (Assert.EnterMultipleScope())
            {
                Assert.That(await StaysPage.OccupancyDropDown.AdultsInputs.GetValueAsync(), Is.EqualTo(adults), "The number of adults is incorrect");
                Assert.That(await StaysPage.OccupancyDropDown.ChildrenInputs.GetValueAsync(), Is.EqualTo(children), "The number of children is incorrect");
                Assert.That(await StaysPage.OccupancyDropDown.GetChildAgeDropDown().GetCurrentValueAsync(), Is.EqualTo("10 years old"), "The child age is incorrect");
                Assert.That(await StaysPage.OccupancyDropDown.RoomsInputs.GetValueAsync(), Is.EqualTo(rooms), "The number of rooms is incorrect");
                Assert.That(await StaysPage.OccupancyDropDown.TravelingWithPetsCheckBox.IsCheckedAsync(), Is.True, "Traveling with pets checkbox is not checked");
            }
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
