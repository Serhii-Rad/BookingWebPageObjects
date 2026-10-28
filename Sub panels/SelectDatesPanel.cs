using BookingWebPageObjects.BaseElements;
using Microsoft.Playwright;
using Microsoft.Testing.Platform.Logging;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.Sub_panels
{
    internal class SelectDatesPanel : SubPanelBase
    {
        private static readonly string panelSelector = "div[data-testid='datepicker-tabs']";
        public SelectDatesPanel(ILocator locator, Button triggerElementToOpen) : base(locator.Locator(panelSelector), triggerElementToOpen)
        {
        }

        public async Task SelectDates(DateTime checkInDate, DateTime checkOutDate)
        {
            await OpenAsync();

            var checkInDateLocator = _panelLocator.Locator($"div[id='calendar-searchboxdatepicker'] span[role='checkbox'][aria-label='{checkInDate.ToString("dddd, MMMM d, yyyy")}']");
            await checkInDateLocator.ClickAsync();
            var checkOutDateLocator = _panelLocator.Locator($"div[id='calendar-searchboxdatepicker'] span[role='checkbox'][aria-label='{checkOutDate.ToString("dddd, MMMM d, yyyy")}']");
            await checkOutDateLocator.ClickAsync();
        }

        public async Task<(DateTime? checkInDate, DateTime? checkOutDate)> GetSelectedDatesAsync()
        {
            await OpenAsync();

            var checkInDate = DateTime.ParseExact(await _panelLocator.Locator("div[id='calendar-searchboxdatepicker'] span[role='checkbox'][aria-checked='true'][aria-label*='Check-in date']").GetAttributeAsync("data-date"), "yyyy-MM-dd", null);
            var checkOutDate = DateTime.ParseExact(await _panelLocator.Locator("div[id='calendar-searchboxdatepicker'] span[role='checkbox'][aria-checked='true'][aria-label*='Check-out date']").GetAttributeAsync("data-date"), "yyyy-MM-dd", null);

            return (checkInDate, checkOutDate);
        }

        public async Task OpenAsync()
        {
            Logs.LogMethodEntry(logLevel: CoreLogLevel.Info);
            if (await IsHiddenAsync())
            {
                if (_triggerElementToOpen is Button inputField)
                    await inputField.ClickAsync();
                else
                    throw new InvalidOperationException("InputField to open is null or not an InputField.");
            }
        }
    }
}
