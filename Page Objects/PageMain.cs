using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Sub_panels;
using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Page_Objects
{
    internal class PageMain : PageBase
    {
        private static readonly string pageSelector = "body";
        public PageMain(IPage page) : base(page.Locator(pageSelector))
        {
        }

        public StaticMenu TabsMenu => new StaticMenu(_pageLocator.Locator("ul[role='menubar']"));
        public CurrencyPanel CurrencyPanel => new CurrencyPanel(_pageLocator, CurrencyButton);
        public LanguagePanel LanguagePanel => new LanguagePanel(_pageLocator, LanguageButton);
        public Button CurrencyButton => new Button(_pageLocator.Locator("button[data-testid='header-currency-picker-trigger']"));
        public Button LanguageButton => new Button(_pageLocator.Locator("button[data-testid='header-language-picker-trigger']"));
        public Button CustomerServiceButton => new Button(_pageLocator.Locator("a[data-testid='header-help-center']"));
        public Button ListYourPropertyButton => new Button(_pageLocator.Locator("a[data-testid='header-custom-action-button']"));
        public Button UserProfileButton => new Button(_pageLocator.Locator("button[data-testid='header-profile']"));
    }
}
