using BookingWebPageObjects.BaseElements;
using Microsoft.Playwright;
using Microsoft.Testing.Platform.Logging;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.Sub_panels
{
    internal class LanguagePanel : SubPanelBase
    {
        private readonly static string panelSelector = "div[id='header_language_picker']";

        public LanguagePanel(ILocator pageLocator, Button triggerElementToOpen) : base(pageLocator.Locator(panelSelector), triggerElementToOpen)
        {
        }

        public async Task SelectLanguage(string language)
        {
            Logs.LogMethodEntry(language, LogLevel.Information);

            var languageOptionLocator = _panelLocator.Locator($"button[data-testid='selection-item']").GetByText(language);
            await languageOptionLocator.ClickAsync();
        }

        public async Task<string> GetSelectedLanguage()
        {
            Logs.LogMethodEntry();

            var selectedLanguageLocator = _panelLocator.Locator("button[data-testid='selection-item'][aria-pressed='true']");
            return await selectedLanguageLocator.InnerTextAsync();
        }

        public async Task<bool> IsVisibleLanguageOption(string language)
        {
            Logs.LogMethodEntry(language);

            var languageOptionLocator = _panelLocator.Locator($"button[data-testid='selection-item']").GetByText(language);
            return await languageOptionLocator.IsVisibleAsync();
        }

        public async Task OpenAsync()
        {
            Logs.LogMethodEntry(logLevel: LogLevel.Information);

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
            Logs.LogMethodEntry(logLevel: LogLevel.Information);

            var closeButtonLocator = _panelLocator.Locator("button[data-testid='selection-modal-close']");
            await closeButtonLocator.ClickAsync();
        }
    }
}
