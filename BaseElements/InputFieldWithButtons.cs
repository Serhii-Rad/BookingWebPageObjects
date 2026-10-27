using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;

namespace BookingWebPageObjects.BaseElements
{
    internal class InputFieldWithButtons : InputField
    {
        public InputFieldWithButtons(ILocator locator) : base(locator)
        {

        }

        public Button PlusButton => new Button(_baseLocator.Locator(".parent button").First);
        public Button MinusButton => new Button(_baseLocator.Locator(".parent button").First);

        public override async Task SetTextAsync(string text, bool withEnterPress = true, bool withClearing = true, int millisecondsInputDelay = 20)
        {
            string? currentValueString = await base.GetValueAsync();

            if (string.IsNullOrEmpty(currentValueString))
            {
                throw new ArgumentNullException(nameof(currentValueString));
            }

            int currentValue = int.Parse(currentValueString);
            int valueToSet = int.Parse(text);

            if (currentValueString == text)
            {
                return;
            }
            else if (currentValue < valueToSet)
            {
                for (int i = 0; i < valueToSet - currentValue; i++)
                {
                    await PlusButton.ClickAsync();
                }
            }
            else
            {
                for (int i = 0; i < currentValue - valueToSet; i++)
                {
                    await MinusButton.ClickAsync();
                }
            }
        
        }

    }
}
