using Microsoft.Playwright;
using TestFrameworkCore.Helpers;

namespace BookingWebPageObjects.Helpers
{
    internal class WaitHelper : WaitHelperBase
    {
        private readonly IPage _page;
        public WaitHelper(IPage page)
        {
            _page = page;
        }
    }
}
