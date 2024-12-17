namespace RR.App.Tests.E2E;

public class PlatformSpecificSampleTest : BaseTest
{
    [Test]
    public void SampleTest()
    {
        App.GetScreenshot().SaveAsFile($"{nameof(SampleTest)}.png");
    }
}