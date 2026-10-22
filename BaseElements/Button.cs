using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebPageObjects.BaseElements
{
    internal class Button : ButtonBase
    {
        public Button(ILocator locator) : base(locator)
        {
        }
    }
}
