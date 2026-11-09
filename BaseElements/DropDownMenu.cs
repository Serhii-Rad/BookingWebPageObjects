using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;

namespace BookingWebPageObjects.BaseElements
{
    internal class DropDownMenu : DropDownMenuBase
    {
        public DropDownMenu(ILocator locator, string selectorThatSelectsAllItems) : base(locator, selectorThatSelectsAllItems)
        {
        }
    }
}
