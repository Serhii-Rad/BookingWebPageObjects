using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;

namespace BookingWebPageObjects.BaseElements
{
    internal class InputField : InputFieldBase
    {
        public InputField(ILocator locator) : base(locator)
        {
        }
    }
}
