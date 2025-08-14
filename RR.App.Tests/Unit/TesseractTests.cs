using RR.App.Service.Interfaces;
using RR.App.Service.Services;

namespace RR.App.Tests.Unit;

public class TesseractTests
{
    readonly IOCRService OcrService = new OCRService();

    [Test]
    public void Tesseract_OCR_Should_Recognize_Text()
    {
        // Arrange
        var imagePath = @"./SampleImages/IMG_20250729_202516.jpg"; // Replace with your test image path
        var expectedText = "Expected text from the image"; // Replace with the expected text
        using (Assert.EnterMultipleScope())
        {
            // Asser that @"./Resources" exists and contains tessdata folder
            Assert.That(Directory.Exists(@"./Resources"), Is.True, "The Resources directory does not exist.");
            Assert.That(Directory.Exists(@"./Resources/tessdata"), Is.True, "The tessdata directory does not exist in the Resources directory.");
            // Assert that the Tesseract language file exists
            var tesseractLanguageFile = Path.Combine(@"./Resources/tessdata", "hun.traineddata");
            Assert.That(File.Exists(tesseractLanguageFile), Is.True, "The Tesseract language file does not exist in the tessdata directory.");
        }

        var imageBytes = File.ReadAllBytes(imagePath);
        // Act
        var recognizedText = OcrService.ExtractTextFromImage(imageBytes);
        // Assert
        Assert.That(recognizedText, Is.EqualTo(expectedText), "The recognized text does not match the expected text.");
    }
}
