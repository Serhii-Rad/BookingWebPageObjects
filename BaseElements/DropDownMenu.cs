using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;

namespace BookingWebPageObjects.BaseElements
{
    internal class DropDownMenu : DropDownMenuBase
    {
        public DropDownMenu(ILocator locator) : base(locator)
        {
        }

        public DropDownMenu(ILocator locator, string anyItemInMenuSelector) : base(locator, anyItemInMenuSelector)
        {
        }
    }
}
