using BookingWebPageObjects.Pages;
using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Page_Objects
{
    internal class Pages 
    {
        private readonly IPage _page;

        private PageMain? _pageMain;
        private StaysPage? _staysPage;

        public Pages(IPage page)
        {
            _page = page;
        }

        public PageMain PageMain => _pageMain ??= new PageMain(_page);
        public StaysPage StaysPage => _staysPage ??= new StaysPage(_page);
    }
}
