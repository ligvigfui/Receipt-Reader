using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using System.Threading.Tasks;

namespace RR.Service.Services;

public class DocumentIntelligenceService(
    IOptions<AzureDocumentIntelligenceAPISettings> options,
    ISecurityService securityService
    ) : IDocumentIntelligenceService
{
    readonly AzureDocumentIntelligenceAPISettings Settings = options.Value;

    public async Task<string> ExtractReceiptDataFromImageAsync(byte[] imageBytes)
    {
        var credential = new AzureKeyCredential(Settings.ApiKey);
        var client = new DocumentAnalysisClient(new Uri(Settings.Endpoint), credential);
        using var stream = new MemoryStream(imageBytes);
        AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-receipt", stream);
        var receipts = operation.Value;
        return ExtractReceiptDataFromTextAsync( receipts );
    }
    public async Task<string> MyExtractReceiptDataFromTextAsync(AnalyzeResult analyzeResult, int? userGroupId)
    {
        foreach (AnalyzedDocument receipt in analyzeResult.Documents)
        {
            var transactionDate = receipt.Fields.GetField(FieldType.TransactionDate)?.AsDate();
            var transactionTime = receipt.Fields.GetField(FieldType.TransactionTime)?.AsTime();

            DateTime? transactionDateTime = (transactionDate, transactionTime) switch
            {
                (DateTimeOffset date, TimeSpan time) => date.Date + time,
                (DateTimeOffset date, null) => date.Date,
                (null, TimeSpan time) => DateTime.Today + time,
                _ => null
            };
            var receiptDBO = new ReceiptDBO()
            {
                User = await securityService.GetUserAsync(),
                GroupId = userGroupId,
                TransactionDateTime = transactionDateTime,
                Vendor = new VendorDBO()
                {
                    Name = receipt.Fields.GetField(FieldType.MerchantName)?.AsString(),
                    Address = new()
                    {
                        StreetAddress = receipt.Fields.GetField(FieldType.MerchantAddress)?.AsString(),
                    }
                },
                Items = receipt.Fields.GetField(FieldType.Items)?.AsList()
                    .Select(item => item.Value.AsDictionary())
                    .Select(item => new ReceiptItemDBO()
                    {
                        OriginalRecognizedName = item.GetField(ItemFieldType.Description)?.AsString(),
                        Product = new ProductDBO()
                        {
                            Name = item.GetField(ItemFieldType.Description)?.AsString(),
                        },
                        Quantity = (float)item.GetField(ItemFieldType.Quantity)?.AsDouble(),
                        PricePerQuantity = (float)item.GetField(ItemFieldType.Price)?.AsCurrency().Amount,
                    }).ToList() ?? [],
            };
        }
        return "";
    }
    public string ExtractReceiptDataFromTextAsync(AnalyzeResult analyzeResult)
    {
        // https://aka.ms/formrecognizer/receiptfields
        var sb = new StringBuilder();
        foreach (AnalyzedDocument receipt in analyzeResult.Documents)
        {
            if (receipt.Fields.TryGetValue("MerchantName", out DocumentField merchantNameField))
            {
                if (merchantNameField.FieldType == DocumentFieldType.String)
                {
                    string merchantName = merchantNameField.Value.AsString();
                    sb.AppendLine($"Merchant Name: '{merchantName}', with confidence {merchantNameField.Confidence}");
                }
            }

            if (receipt.Fields.TryGetValue("TransactionDate", out DocumentField transactionDateField))
            {
                if (transactionDateField.FieldType == DocumentFieldType.Date)
                {
                    var transactionDate = transactionDateField.Value.AsDate();
                    sb.AppendLine($"Transaction Date: '{transactionDate}', with confidence {transactionDateField.Confidence}");
                }
            }

            if (receipt.Fields.TryGetValue("Items", out DocumentField itemsField))
            {
                if (itemsField.FieldType == DocumentFieldType.List)
                {
                    foreach (DocumentField itemField in itemsField.Value.AsList())
                    {
                        sb.AppendLine("Item:");
                        if (itemField.FieldType == DocumentFieldType.Dictionary)
                        {
                            var itemFields = itemField.Value.AsDictionary();
                            if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField))
                            {
                                if (itemDescriptionField.FieldType == DocumentFieldType.String)
                                {
                                    string itemDescription = itemDescriptionField.Value.AsString();
                                    sb.AppendLine($"  Description: '{itemDescription}', with confidence {itemDescriptionField.Confidence}");
                                }
                            }
                            if (itemFields.TryGetValue("TotalPrice", out DocumentField itemTotalPriceField))
                            {
                                if (itemTotalPriceField.FieldType == DocumentFieldType.Currency)
                                {
                                    var currency = itemTotalPriceField.Value.AsCurrency();
                                    double? itemTotalPrice = currency.Amount;
                                    sb.AppendLine($"  Total Price: '{itemTotalPrice}', with confidence {itemTotalPriceField.Confidence}");
                                }
                            }
                        }
                    }
                }
            }

            if (receipt.Fields.TryGetValue("Total", out DocumentField totalField))
            {
                if (totalField.FieldType == DocumentFieldType.Currency)
                {
                    var currency = totalField.Value.AsCurrency();
                    double? total = currency.Amount;
                    sb.AppendLine($"Total: '{total}', with confidence '{totalField.Confidence}'");
                }
            }
        }
        return sb.ToString();
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