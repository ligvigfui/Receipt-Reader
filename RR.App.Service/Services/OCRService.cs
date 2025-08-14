using Tesseract;

namespace RR.App.Service.Services;

public class OCRService : IOCRService
{
    public string ExtractTextFromImage(byte[] imageBytes)
    {
        using var engine = new TesseractEngine(@"./Resources/tessdata", AppConfig.TesseractLanguage, EngineMode.Default);
        using var img = Pix.LoadFromMemory(imageBytes);
        using var page = engine.Process(img);
        // Get the text from the page
        var text = page.GetText();
        // Return the recognized text
        return text.Trim();

    }
}
