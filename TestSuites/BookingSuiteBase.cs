using BookingWebPageObjects.Page_Objects;
using BookingWebPageObjects.Pages;
using NUnit.Framework;
using PlaywrightBaseFramework.Helpers.Extensions;
using PlaywrightBaseFramework.TestSuite;

namespace BookingWebPageObjects.TestSuites
{
    [TestFixture]
    internal class BookingSuiteBase : SuiteBase
    {
        private const string BaseUrl = "https://www.booking.com/";
        public BookingSuiteBase() : base()
        {
        }

        [SetUp]
        private async Task TestStartUp()
        {
            await Page.NavigateAsync(BaseUrl);
        }

        public Page_Objects.Pages Pages => new Page_Objects.Pages(Page);

        #region Pages
        public PageMain PageMain => Pages.PageMain;
        public StaysPage StaysPage => Pages.StaysPage;
        #endregion
    }
}
