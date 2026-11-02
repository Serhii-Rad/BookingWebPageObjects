using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Sub_panels;
using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Page_Objects
{
    internal class PageMain : PageBase
    {
        private static readonly string _pageSelector = "body";

        private StaticMenu? _tabsMenu;
        private CurrencyPanel? _currencyPanel;
        private LanguagePanel? _languagePanel;
        private Button? _currencyButton;
        private Button? _languageButton;
        private Button? _customerServiceButton;
        private Button? _listYourPropertyButton;
        private Button? _userProfileButton;
        private DropDownMenu? _userDropDownMenu;

        public PageMain(IPage page) : base(page.Locator(_pageSelector))
        {
        }

        public StaticMenu TabsMenu => _tabsMenu ??= new StaticMenu(_pageLocator.Locator("ul[role='menubar']"), "a[role='menuitem']");
        public CurrencyPanel CurrencyPanel => _currencyPanel ??= new CurrencyPanel(_pageLocator, CurrencyButton);
        public LanguagePanel LanguagePanel => _languagePanel ??= new LanguagePanel(_pageLocator, LanguageButton);
        public Button CurrencyButton => _currencyButton ??= new Button(_pageLocator.Locator("button[data-testid='header-currency-picker-trigger']"));
        public Button LanguageButton => _languageButton ??= new Button(_pageLocator.Locator("button[data-testid='header-language-picker-trigger']"));
        public Button CustomerServiceButton => _customerServiceButton ??= new Button(_pageLocator.Locator("a[data-testid='header-help-center']"));
        public Button ListYourPropertyButton => _listYourPropertyButton ??= new Button(_pageLocator.Locator("a[data-testid='header-custom-action-button']"));
        public Button UserProfileButton => _userProfileButton ??= new Button(_pageLocator.Locator("button[data-testid='header-profile']"));
        public DropDownMenu UserDropDownMenu => _userDropDownMenu ??= new DropDownMenu(_pageLocator.Locator("div[data-testid='header-profile-menu']"), "li");
    }
}
