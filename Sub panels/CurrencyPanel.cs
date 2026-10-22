using BookingWebPageObjects.BaseElements;
using Microsoft.Playwright;
using Microsoft.Testing.Platform.Logging;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.Sub_panels
{
    internal class CurrencyPanel : SubPanelBase
    {
        private readonly static string panelSelector = "div[id='header_currency_picker']";
        public CurrencyPanel(ILocator pageLocator, Button triggerElementToOpen) : base(pageLocator.Locator(panelSelector), triggerElementToOpen)
        {
        }

        public async Task SelectCurrency(string currencyCode)
        {
            Logs.LogMethodEntry(currencyCode, LogLevel.Information);

            var currencyOptionLocator = _panelLocator.Locator($"button[data-testid='selection-item']").GetByText(currencyCode);
            await currencyOptionLocator.ClickAsync();
        }

        public async Task<string> GetSelectedCurrency()
        {
            Logs.LogMethodEntry();

            var selectedCurrencyLocator = _panelLocator.Locator("button[data-testid='selection-item'][aria-pressed='true']");
            return await selectedCurrencyLocator.InnerTextAsync();
        }

        public async Task<bool> IsVisibleCurrencyOption(string currencyCode)
        {
            Logs.LogMethodEntry(currencyCode);

            var currencyOptionLocator = _panelLocator.Locator($"button[data-testid='selection-item']").GetByText(currencyCode);
            return await currencyOptionLocator.IsVisibleAsync();
        }

        public async Task OpenAsync()
        {
            Logs.LogMethodEntry(logLevel: LogLevel.Information);

            if (_triggerElementToOpen is Button button)
                await button.ClickAsync();
            else
                throw new InvalidOperationException("Button to open is null or not a Button.");
        }

        public async Task CloseAsync()
        {
            Logs.LogMethodEntry(logLevel: LogLevel.Information);

            var closeButtonLocator = _panelLocator.Locator("button[data-testid='selection-modal-close']");
            await closeButtonLocator.ClickAsync();
        }
    }
}
