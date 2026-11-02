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
            Logs.LogMethodEntry($"Locator: {_panelLocator}; Check-in Date: {checkInDate.ToString("yyyy-MM-dd")}; Check-out Date: {checkOutDate.ToString("yyyy-MM-dd")}", CoreLogLevel.Info);
            await OpenAsync();

            var checkInDateLocator = _panelLocator.Locator($"div[id='calendar-searchboxdatepicker'] span[role='checkbox'][aria-label='{checkInDate.ToString("dddd, MMMM d, yyyy")}']");
            await checkInDateLocator.ClickAsync();
            var checkOutDateLocator = _panelLocator.Locator($"div[id='calendar-searchboxdatepicker'] span[role='checkbox'][aria-label='{checkOutDate.ToString("dddd, MMMM d, yyyy")}']");
            await checkOutDateLocator.ClickAsync();
        }

        public async Task<(DateTime? checkInDate, DateTime? checkOutDate)> GetSelectedDatesAsync()
        {
            Logs.LogMethodEntry($"Locator: {_panelLocator}");
            await OpenAsync();

            string? checkInDateString = await _panelLocator.Locator("div[id='calendar-searchboxdatepicker'] span[role='checkbox'][aria-checked='true'][aria-label*='Check-in date']").GetAttributeAsync("data-date");
            string? checkOutDateString = await _panelLocator.Locator("div[id='calendar-searchboxdatepicker'] span[role='checkbox'][aria-checked='true'][aria-label*='Check-out date']").GetAttributeAsync("data-date");

            (DateTime? checkInDate, DateTime? checkOutDate) returnValues = (null, null);

            if (!string.IsNullOrEmpty(checkInDateString))
                returnValues.checkInDate = DateTime.ParseExact(checkInDateString, "yyyy-MM-dd", null);
            if (!string.IsNullOrEmpty(checkOutDateString))
                returnValues.checkOutDate = DateTime.ParseExact(checkOutDateString, "yyyy-MM-dd", null);

            return returnValues;
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
