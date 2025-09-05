namespace RR.Service.Interfaces;

public interface IDocumentIntelligenceService
{
    Task<string> ExtractReceiptDataFromImageAsync(byte[] imageBytes);
}
