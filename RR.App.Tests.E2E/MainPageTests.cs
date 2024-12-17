﻿namespace RR.App.Tests.E2E;

// This is an example of tests that do not need anything platform specific
public class MainPageTests : BaseTest
{
    [Test]
    public void AppLaunches()
    {
        App.GetScreenshot().SaveAsFile($"{nameof(AppLaunches)}.png");
    }

    [Test]
    public void ClickCounterTest()
    {
        // Arrange
        // Find elements with the value of the AutomationId property
        var element = FindUIElement("CounterBtn");

        // Act
        element.Click();
        Task.Delay(500).Wait(); // Wait for the click to register and show up on the screenshot

        // Assert
        App.GetScreenshot().SaveAsFile($"{nameof(ClickCounterTest)}.png");
        Assert.That(element.Text, Is.EqualTo("Clicked 1 time"));
    }
}