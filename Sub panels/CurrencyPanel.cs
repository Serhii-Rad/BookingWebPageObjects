using BookingWebPageObjects.BaseElements;
using Microsoft.Playwright;
using Microsoft.Testing.Platform.Logging;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.Sub_panels
{
    internal class CurrencyPanel : SubPanelBase
    {
        private readonly static string panelSelector = "div[id='header_currency_picker'][role='dialog']";
        public CurrencyPanel(ILocator pageLocator, Button triggerElementToOpen) : base(pageLocator.Locator(panelSelector), triggerElementToOpen)
        {
        }

        public async Task SelectCurrency(string currencyCode)
        {
            Logs.LogMethodEntry($"Locator: {_panelLocator}; Currency Code: {currencyCode}", CoreLogLevel.Info);

            await OpenAsync();

            var currencyOptionLocator = _panelLocator.Locator($"button[data-testid='selection-item']").GetByText(currencyCode).First;
            await currencyOptionLocator.ClickAsync();
        }

        public async Task<string> GetSelectedCurrency()
        {
            Logs.LogMethodEntry($"Locator: {_panelLocator}");

            await OpenAsync();

            var selectedCurrencyLocator = _panelLocator.Locator("button[data-testid='selection-item'][aria-pressed='true']");
            return await selectedCurrencyLocator.InnerTextAsync();
        }

        public async Task<bool> IsVisibleCurrencyOption(string currencyCode)
        {
            Logs.LogMethodEntry($"Locator: {_panelLocator}; Currency Code: {currencyCode}");

            var currencyOptionLocator = _panelLocator.Locator($"button[data-testid='selection-item']").GetByText(currencyCode);
            return await currencyOptionLocator.IsVisibleAsync();
        }

        public async Task OpenAsync()
        {
            Logs.LogMethodEntry($"Locator: {_panelLocator}", CoreLogLevel.Info);

            if (await IsHiddenAsync())
            {
                if (_triggerElementToOpen is Button button)
                    await button.ClickAsync();
                else
                    throw new InvalidOperationException("Button to open is null or not a Button.");
            }
        }

        public async Task CloseAsync()
        {
            Logs.LogMethodEntry($"Locator: {_panelLocator}", CoreLogLevel.Info);

            if (await IsVisibleAsync())
            {
                var closeButtonLocator = _panelLocator.Locator("button[data-testid='selection-modal-close']");
                await closeButtonLocator.ClickAsync();
            }
        }
    }
}
