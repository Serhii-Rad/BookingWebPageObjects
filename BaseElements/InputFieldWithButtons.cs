using Microsoft.Playwright;
using PlaywrightBaseFramework.BaseElements;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.BaseElements
{
    internal class InputFieldWithButtons : InputFieldBase
    {
        public InputFieldWithButtons(ILocator locator) : base(locator)
        {

        }

        public ButtonBase PlusButton => new ButtonBase(_baseLocator.Locator("~button").Last);
        public ButtonBase MinusButton => new ButtonBase(_baseLocator.Locator("~button").First);

        protected override async Task SetTextLogicAsync(string text, bool withEnterPress = true, bool withClearing = true, int millisecondsInputDelay = 20)
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
