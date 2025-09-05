using RR.Service.Services;

namespace RR.Tests;

public class DocumentIntelligenceServiceTests
{
    [Test]
    public async Task ExtractReceiptDataFromImageAsync_ShouldReturnResult()
    {
        // Arrange
        var service = new DocumentIntelligenceService();
        var imagePath = Path.Combine("SampleImages", "IMG_20250729_202516.jpg");
        var imageBytes = await File.ReadAllBytesAsync(imagePath);

        // Act
        var result = await service.ExtractReceiptDataFromImageAsync(imageBytes);

    }
}
