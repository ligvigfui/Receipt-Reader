namespace RR.App.Tests.E2E;

public abstract class BaseTest
{
    protected AppiumDriver App => AppiumSetup.App;

    // This could also be an extension method to AppiumDriver if you prefer
    protected AppiumElement FindUIElement(string id)
    {
        return App.FindElement(MobileBy.AccessibilityId(id));
    }
}