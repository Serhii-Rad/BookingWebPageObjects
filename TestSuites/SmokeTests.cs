using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Page_Objects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFrameworkCore.Asserts;
using TestFrameworkCore.Helpers.Attributes;

namespace BookingWebPageObjects.TestSuites
{
    [TestFixture]
    internal class SmokeTests : BookingSuiteBase
    {
       

        [Test]
        public async Task TC0001_TestAllExpectedTabsButtonsArePresent()
        {
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
    }
}
