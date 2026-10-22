using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebPageObjects.Sub_panels
{
    internal class OccupancyDropDownPanel : SubPanelBase
    {
        private static readonly string panelSelector = "div[data-testid='occupancy-dropdown']";
        public OccupancyDropDownPanel(ILocator pageLocator) : base(pageLocator.Locator(panelSelector))
        {
        }
    }
}
