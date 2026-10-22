using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Page_Objects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFrameworkCore.Asserts;

namespace BookingWebPageObjects.TestSuites
{
    [TestFixture]
    internal class SmokeTests : BookingSuiteBase
    {
        [Test]
        public async Task TestAllExpectedTabsButtonsArePresent()
        {
            List<string> expectedTabsNames = new List<string> { "Stays", "Flights", "Flight + Hotel", "Car rental", "Attractions", "Airport transfers" };
            List<string?> actualTabsNames = (await PageMain.TabsMenu.GetMenuElementsAsync<Button>()).Select(x => x.GetTextAsync().Result).ToList();
            TestAsserts.AssertCollectionAreEquivalent(expectedTabsNames, actualTabsNames, "There are incorrect main tabs items");
        }   
    }
}
