using BookingWebPageObjects.Pages;
using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Page_Objects
{
    internal class Pages : PagesBase
    {

        public Pages(IPage page) : base(page) 
        {
        }

        public PageMain PageMain => GetInstance<PageMain>();

        public StaysPage StaysPage => GetInstance<StaysPage>();
    }
}
