using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace RR.App.Tests.E2E;

[SetUpFixture]
public class AppiumSetup
{
#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
    private static AppiumDriver? driver;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

    public const string DefaultHostAddress = "127.0.0.1";
    public const int DefaultHostPort = 4723;

    public static AppiumDriver App => driver ?? throw new NullReferenceException("AppiumDriver is null");

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        // If you started an Appium server manually, make sure to comment out the next line
        // This line starts a local Appium server for you as part of the test run
        AppiumServerHelper.StartAppiumLocalServer(DefaultHostAddress, DefaultHostPort);
        var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_HOST") ?? $"http://{DefaultHostAddress}:{DefaultHostPort}/");
        var driverOptions = new AppiumOptions()
        {
            AutomationName = AutomationName.AndroidUIAutomator2,
            PlatformName = "Android",
            DeviceName = "Android Emulator",
        };

        driverOptions.AddAdditionalAppiumOption("appPackage", "com.ligvigfui.ReceiptReader");
        driverOptions.AddAdditionalAppiumOption("appActivity", ".MainActivity");
        // NoReset assumes the app com.google.android is preinstalled on the emulator
        driverOptions.AddAdditionalAppiumOption("noReset", true);

        driver = new AndroidDriver(serverUri, driverOptions, TimeSpan.FromSeconds(180));
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        App.ActivateApp("com.ligvigfui.ReceiptReader");
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        driver?.Quit();

        // If an Appium server was started locally above, make sure we clean it up here
        AppiumServerHelper.DisposeAppiumLocalServer();
    }
}