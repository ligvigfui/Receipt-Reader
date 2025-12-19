namespace RR.Tests;

public class DocumentIntelligenceServiceTests
{
    [Test]
    public async Task ExtractReceiptDataFromImageAsync_ShouldReturnResult()
    {
        // Arrange
        // get the appsettings.secrets.json file from the RR.API project
        var configuration = Get.Configuration();
        var azureSettings = configuration.GetSection("AzureDocumentIntelligenceAPI").Get<AzureDocumentIntelligenceAPISettings>();
        var appDBContext = Get.ApplicationDbContext();
        var measurementRepository = new MeasurementRepository(appDBContext);
        await measurementRepository.CreateMeasurement(new CreateMeasurement()
        {
            Name = "Darab",
            Plural = "Darab",
            Symbol = "db",
            Category = MeasurementCategory.Count,
            ConversionFactorToSI = 1
        });
        var service = new DocumentIntelligenceService(Options.Create(azureSettings), measurementRepository);
        var imagePath = Path.Combine("SampleImages", "IMG_20250729_202516.jpg");
        var imageBytes = await File.ReadAllBytesAsync(imagePath);

        // Act
        var result = await service.ExtractReceiptDataFromImageAsync(imageBytes, null);

    }
}
