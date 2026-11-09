using BookingWebPageObjects.BaseElements;
using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.Sub_panels
{
    /// <summary>
    /// Calendar popup on the Flights page. Closes automatically after the last date is selected (2 dates for Round-trip, 1 for One-way).
    /// </summary>
    internal class FlightsDatesPanel : SubPanelBase
    {
        private static readonly string panelSelector = "div[data-bui-trap-root]:has([data-ui-name='calendar_body'])";
        private const int maxMonthsToScroll = 12;
        private const int dateCellWaitTimeout = 1000;

        public FlightsDatesPanel(ILocator pageLocator, Button triggerElementToOpen) : base(pageLocator.Locator(panelSelector), triggerElementToOpen)
        {
        }

        public Button NextMonthButton => new Button(_panelLocator.Locator("button[aria-label='Next month']"));
        public Button PreviousMonthButton => new Button(_panelLocator.Locator("button[aria-label='Previous month']"));

        public Button GetDateCell(DateTime date) => new Button(_panelLocator.Locator($"span[data-ui-name='calendar_cell_{date:yyyy-MM-dd}']"));

        /// <summary>
        /// For One-way trip leave returnDate null.
        /// </summary>
        public async Task SelectDatesAsync(DateTime departureDate, DateTime? returnDate = null)
        {
            Logs.LogMethodEntry($"Departure: {departureDate:yyyy-MM-dd}; Return: {returnDate:yyyy-MM-dd}", CoreLogLevel.Info);

            await OpenAsync();
            // wait for the calendar to be rendered, otherwise the date cell is not found and the calendar is scrolled too far
            await NextMonthButton.IsVisibleAsync();
            await SelectDateAsync(departureDate);
            if (returnDate.HasValue)
                await SelectDateAsync(returnDate.Value);
        }

        private async Task SelectDateAsync(DateTime date)
        {
            var dateCell = GetDateCell(date);
            for (int i = 0; i < maxMonthsToScroll && !await dateCell.IsVisibleAsync(dateCellWaitTimeout); i++)
                await NextMonthButton.ClickAsync();

            await dateCell.ClickAsync();
        }

        public async Task OpenAsync()
        {
            Logs.LogMethodEntry(logLevel: CoreLogLevel.Info);
            if (await IsHiddenAsync())
            {
                if (_triggerElementToOpen is Button button)
                    await button.ClickAsync();
                else
                    throw new InvalidOperationException("Button to open is null or not a Button.");
            }
        }
    }
}
