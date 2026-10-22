using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebPageObjects.BaseElements
{
    internal class DropDownMenu : DropDownMenuBase
    {
        public DropDownMenu(ILocator locator) : base(locator)
        {
        }
    }
}
