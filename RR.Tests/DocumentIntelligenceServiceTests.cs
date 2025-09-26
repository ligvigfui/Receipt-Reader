using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RR.Service.Services;
using RR.Settings;

namespace RR.Tests;

public class DocumentIntelligenceServiceTests
{
    [Test]
    public async Task ExtractReceiptDataFromImageAsync_ShouldReturnResult()
    {
        // Arrange
        // get the appsettings.secrets.json file from the RR.API project
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.secrets.json")
            .Build();
        var azureSettings = configuration.GetSection("AzureDocumentIntelligenceAPI").Get<AzureDocumentIntelligenceAPISettings>();
        var service = new DocumentIntelligenceService(Options.Create(azureSettings));
        var imagePath = Path.Combine("SampleImages", "IMG_20250729_202516.jpg");
        var imageBytes = await File.ReadAllBytesAsync(imagePath);

        // Act
        var result = await service.ExtractReceiptDataFromImageAsync(imageBytes);

    }
}
