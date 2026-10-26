using Microsoft.Playwright;
using PlaywrightBaseFramework.PageCore;
using TestFrameworkCore.Interfaces;

namespace BookingWebPageObjects.Sub_panels
{
    /// <summary>
    /// Any panel or dropdown that is opened by a trigger element (ex. button click)
    /// </summary>
    internal class SubPanelBase : PageBase
    {
        protected readonly ILocator _panelLocator;
        protected readonly IElementBase? _triggerElementToOpen = null;

        /// <summary>
        /// Default constructor for sub-panel. It is used when sub-panel has a unique selector.
        /// </summary>
        /// <param name="panelLocator"></param>
        public SubPanelBase(ILocator panelLocator) : base(panelLocator)
        {
            _panelLocator = panelLocator;
        }

        /// <summary>
        /// Constructor for sub-panel that needs to be opened by a trigger element (ex. button click).
        /// </summary>
        /// <param name="panelLocator"></param>
        /// <param name="triggerToOpen"></param>
        public SubPanelBase(ILocator panelLocator, IElementBase triggerElementToOpen) : base(panelLocator)
        {
            _panelLocator = panelLocator;
            _triggerElementToOpen = triggerElementToOpen;
        }
    }
}
