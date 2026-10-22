using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebPageObjects.BaseElements
{
    internal class CheckBox : CheckBoxBase
    {
        public CheckBox(ILocator locator) : base(locator)
        {
        }
    
    }
}
