using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace RR.Service.Services;

public class DocumentIntelligenceService(
    IOptions<AzureDocumentIntelligenceAPISettings> options,
    IMeasurementRepository measurementRepository
) : IDocumentIntelligenceService
{
    readonly string ModelName = options.Value.Model;
    readonly DocumentAnalysisClient DAClient = new (new Uri(options.Value.Endpoint), new AzureKeyCredential(options.Value.ApiKey));

    public async Task<Receipt> ExtractReceiptDataFromImageAsync(Uri imageURI, int? userGroupId)
    {
        var analizeResult = await DAClient.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, ModelName, imageURI);
        return await MyExtractReceiptDataFromTextAsync(analizeResult, userGroupId);
    }
    public async Task<Receipt> ExtractReceiptDataFromImageAsync(byte[] imageBytes, int? userGroupId)
    {
        using var stream = new MemoryStream(imageBytes);
        var analizeResult = await DAClient.AnalyzeDocumentAsync(WaitUntil.Completed, ModelName, stream);
        return await MyExtractReceiptDataFromTextAsync(analizeResult, userGroupId);
    }
    async Task<Receipt> MyExtractReceiptDataFromTextAsync(AnalyzeDocumentOperation analizeResult, int? userGroupId)
    {
        var receipt = analizeResult.Value.Documents[0];
        var language = analizeResult.Value.Languages.FirstOrDefault()?.Locale;
        var transactionDate = receipt.Fields.GetField(FieldType.TransactionDate)?.AsDate();
        var transactionTime = receipt.Fields.GetField(FieldType.TransactionTime)?.AsTime();

        DateTime? transactionDateTime = (transactionDate, transactionTime) switch
        {
            (DateTimeOffset date, TimeSpan time) => date.Date + time,
            (DateTimeOffset date, null) => date.Date,
            (null, TimeSpan time) => DateTime.Today + time,
            _ => null
        };
        var address = receipt.Fields.GetField(FieldType.MerchantAddress)?.AsAddress();
        var items = receipt.Fields.GetField(FieldType.Items)?.AsList()
                .Select(item => item.Value.AsDictionary());
        var receiptItems = new List<ReceiptItem>();
        var measurementsToGet = new Dictionary<string, List<int>>();
        var index = -1;
        foreach (var item in items)
        {
            index++;
            var quantity = (float?)item.GetField(ItemFieldType.Quantity)?.AsDouble();
            if (quantity is null)
            {
                receiptItems.Add(new ReceiptItem()
                {
                    Name = item.GetField(ItemFieldType.Description)?.AsString(),
                    Quantity = 1,
                    PricePerQuantity = (float)item.GetField(ItemFieldType.TotalPrice).AsDouble()
                });
                continue;
            }
            var measurement = item.GetField(ItemFieldType.QuantityUnit)?.AsString().ToLower();
            if (measurementsToGet.TryGetValue(measurement, out var value))
                value.Add(index);
            measurementsToGet.Add(measurement, [index]);
            
            receiptItems.Add(new ReceiptItem()
            {
                Name = item.GetField(ItemFieldType.Description)?.AsString(),
                Quantity = quantity!.Value,
                PricePerQuantity = (float)item.GetField(ItemFieldType.Price).AsDouble()
            });
        }
        var measurements = await measurementRepository.GetMeasurements(measurementsToGet.Keys.ToList());
        foreach (var measurement in measurements)
            foreach (var itemIndex in measurementsToGet[measurement.Symbol.ToLower()])
                receiptItems[itemIndex].Measurement = measurement;

        return new Receipt()
        {
            GroupId = userGroupId,
            TransactionDateTime = transactionDateTime,
            Vendor = new Vendor()
            {
                VendorHQ = new()
                {
                    Name = receipt.Fields.GetField(FieldType.MerchantName)?.AsString(),
                },
                Address = new()
                {
                    Country = address.State,
                    Region = address.StateDistrict + address.CountryRegion,
                    PostalCode = address.PostalCode,
                    City = address.City,
                    StreetAddress = address.StreetAddress,
                }
            },
            Items = receiptItems
        };
    }
}
public enum ItemFieldType
{
    Description,
    Price,
    Quantity,
    QuantityUnit,
    TotalPrice,
}
public enum FieldType
{
    ReceiptType,
    MerchantName,
    MerchantPhoneNumber,
    MerchantAddress,
    TransactionDate,
    TransactionTime,
    Total,
    Subtotal,
    Tax,
    Tip,
    Items,
    Name,
    Quantity,
    Price,
    TotalPrice,
}

public static class DocumentFiledExtensions
{
    public static DocumentFieldValue? GetField(this IReadOnlyDictionary<string, DocumentField> fields, FieldType fieldType) =>
        fields.TryGetValue(fieldType.ToString(), out var field) ? field?.Value : null;
    public static DocumentFieldValue? GetField(this IReadOnlyDictionary<string, DocumentField> fields, ItemFieldType fieldType) =>
        fields.TryGetValue(fieldType.ToString(), out var field) ? field?.Value : null;
}