namespace RR.Service.Interfaces;

public interface IDocumentIntelligenceService
{
    Task<Receipt> ExtractReceiptDataFromImageAsync(byte[] imageBytes, int? userGroupId);
    Task<Receipt> ExtractReceiptDataFromImageAsync(Uri imageURI, int? userGroupId);
}
