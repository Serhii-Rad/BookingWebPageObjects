using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;

namespace BookingWebPageObjects.BaseElements
{
    internal class DateTimeCalendar : DateTimePickerBase
    {
        public DateTimeCalendar(ILocator locator) : base(locator)
        {
        }
    }
}
