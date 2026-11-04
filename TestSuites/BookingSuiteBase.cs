using BookingWebPageObjects.Helpers;
using BookingWebPageObjects.Page_Objects;
using BookingWebPageObjects.Pages;
using NUnit.Framework;
using PlaywrightBaseFramework.Helpers.Extensions;
using PlaywrightBaseFramework.TestSuite;
using System.Runtime.InteropServices;
using TestFrameworkCore.Helpers;
using TestFrameworkCore.Helpers.Attributes;
using TestFrameworkCore.LoggerLogs;

namespace BookingWebPageObjects.TestSuites
{
    [TestFixture]
    internal class BookingSuiteBase : SuiteBase
    {
        public static string? BaseUrl { get; private set; } = TestContext.Parameters.Get("baseUrl");
        public static string? DefaultUserName { get; private set; } = TestContext.Parameters.Get("defaultUserName");
        public static string? DefaultPassword { get; private set; } = TestContext.Parameters.Get("defaultPassword");

        public WaitHelper WaitHelper => new WaitHelper(Page);

        [OneTimeSetUp]
        public void BookingOneTimeSetUp()
        {
            // Add any necessary actions here
        }

        [SetUp]
        public async Task BookingEveryTestSetUp()
        {
            await CreatePageAsync();

            var startupCustomMethodTask = GetCustomStartUpMethod()?.Invoke(this, null);
            if (startupCustomMethodTask is Task task)
                await task;

            // Add any necessary actions here

            TestContext.Out.WriteLine($"{nameof(BookingEveryTestSetUp)} finished: '{_test.Name}'.");
            Logs.Logger.Info($"{nameof(BookingEveryTestSetUp)} finished: '{_test.Name}'.\n");

        }

        [OneTimeTearDown]
        public void BookingOneTimeTearDown()
        {
            // Add any necessary actions here

            TestContext.Out.WriteLine($"{nameof(BookingOneTimeTearDown)} finished: '{_test.Name}'.");
            Logs.Logger.Info($"{nameof(BookingOneTimeTearDown)} finished: '{_test.Name}'.\n");
        }

        [TearDown]
        public void BookingEveryTestTearDown()
        {
            // Add any necessary actions here

            TestContext.Out.WriteLine($"{nameof(BookingEveryTestTearDown)} finished: '{_test.Name}'.");
            Logs.Logger.Info($"{nameof(BookingEveryTestTearDown)} finished: '{_test.Name}'.\n");
        }

        protected override async Task DefaultTestSetUpAsync()
        {
            if (!string.IsNullOrEmpty(BaseUrl))
                await Page.GotoAsync(BaseUrl);

            // skip sign in popup
            if (await Page.Locator("div[aria-label='Window offering discounts of 10% or more when you sign in to Booking.com']").WaitForConditionAsync(x => x.IsVisibleAsync(), Timeouts.Medium))
                await Page.Locator("button[aria-label='Dismiss sign-in info.']").ClickAsync();

            TestContext.Out.WriteLine($"BookingDefaultStartUpAsync finished: '{_test.Name}'.");
            Logs.Logger.Info($"BookingDefaultStartUpAsync finished: '{_test.Name}'.\n");
        }

        private Page_Objects.Pages? _pages;
        public Page_Objects.Pages Pages => _pages ??= new Page_Objects.Pages(Page); // is needed to get access to all pages and page objects in any point of the test suite, without creating new instances of pages and page objects.

        /// <summary>
        ///  All the pages and page objects are listed here for easy access. You can use them in your tests like this: PageMain.SomeMethod(), StaysPage.SomeMethod(), instead of Pages.PageMain.SomeMethod(), Pages.StaysPage.SomeMethod(). This is just for convenience and readability of the tests.
        /// </summary>
        #region Pages
        public PageMain PageMain => Pages.PageMain;
        public StaysPage StaysPage => Pages.StaysPage;
        public FlightsPage FlightsPage => Pages.FlightsPage;
        #endregion
    }
}


// Explanation about setup/teardown method ierarhy
// BaseOneTimeSetup - One time setup method from base suite class
//     BookingOneTimeSetUpAsync - One time setup method from child suite class
//         BaseEveryTestSetupAsync - Setup method for every test case from base suite class
//            BookingEveryTestSetUpAsync - Setup method for every test case from child suite class
//                DefaultTestSetUpAsync - Overwritten method from child suite class. Why we use approach with this method instead of just using defaults setups? Because this method can be overwritten for each test case independently, and in case one test case needs some special login logic - we just create a new method which will be used only for the needed test case automatically, replaced DefaultTestSetUpAsync logic.
//            BookingEveryTestTearDownAsync - Teardown method for every test case from child suite class
//         BaseEveryTestTearDownAsync - Teardown method for every test case from base suite class
//     BookingOneTimeTearDownAsync - One time teardown method for every test case from child suite class
// BaseOneTimeTearDown - One time teardown method for every test case from base suite class