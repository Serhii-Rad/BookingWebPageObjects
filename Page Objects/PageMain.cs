using BookingWebPageObjects.BaseElements;
using BookingWebPageObjects.Sub_panels;
using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;
using PlaywrightBaseFramework.PageCore;

namespace BookingWebPageObjects.Page_Objects
{
    internal class PageMain : PageBase
    {
        private static readonly string _pageSelector = "body";

        private StaticMenuBase? _tabsMenu;
        private CurrencyPanel? _currencyPanel;
        private LanguagePanel? _languagePanel;
        private ButtonBase? _currencyButton;
        private ButtonBase? _languageButton;
        private ButtonBase? _customerServiceButton;
        private ButtonBase? _listYourPropertyButton;
        private ButtonBase? _userProfileButton;
        private DropDownMenuBase? _userDropDownMenu;

        public PageMain(IPage page) : base(page.Locator(_pageSelector))
        {
        }

        public StaticMenuBase TabsMenu => _tabsMenu ??= new StaticMenuBase(_pageLocator.Locator("ul[role='menubar']"), "a[role='menuitem']");
        public CurrencyPanel CurrencyPanel => _currencyPanel ??= new CurrencyPanel(_pageLocator, CurrencyButton);
        public LanguagePanel LanguagePanel => _languagePanel ??= new LanguagePanel(_pageLocator, LanguageButton);
        public ButtonBase CurrencyButton => _currencyButton ??= new ButtonBase(_pageLocator.Locator("button[data-testid='header-currency-picker-trigger']"));
        public ButtonBase LanguageButton => _languageButton ??= new ButtonBase(_pageLocator.Locator("button[data-testid='header-language-picker-trigger']"));
        public ButtonBase CustomerServiceButton => _customerServiceButton ??= new ButtonBase(_pageLocator.Locator("a[data-testid='header-help-center']"));
        public ButtonBase ListYourPropertyButton => _listYourPropertyButton ??= new ButtonBase(_pageLocator.Locator("a[data-testid='header-custom-action-button']"));
        public ButtonBase UserProfileButton => _userProfileButton ??= new ButtonBase(_pageLocator.Locator("button[data-testid='header-profile']"));
        public DropDownMenuBase UserDropDownMenu => _userDropDownMenu ??= new DropDownMenuBase(_pageLocator.Locator("div[data-testid='header-profile-menu']"), "li");
    }
}
