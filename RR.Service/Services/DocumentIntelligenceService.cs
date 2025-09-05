using Azure;
using Azure.AI.DocumentIntelligence;
//using Azure.AI.FormRecognizer.DocumentAnalysis;
namespace RR.Service.Services;

public class DocumentIntelligenceService : IDocumentIntelligenceService
{
    // Replace with your actual endpoint and key, or inject via configuration

    public async Task<string> ExtractReceiptDataFromImageAsync(byte[] imageBytes)
    {
        var credential = new AzureKeyCredential(apiKey);
        var client = new DocumentIntelligenceClient(new Uri(endpoint), credential);
            
        var binaryData = new BinaryData(imageBytes);
        var operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-receipt", binaryData);

        AnalyzeResult receipts = operation.Value;

        // To see the list of the supported fields returned by service and its corresponding types, consult:
        // https://aka.ms/formrecognizer/receiptfields
        var sb = new StringBuilder();
        foreach (AnalyzedDocument receipt in receipts.Documents)
        {
            if (receipt.Fields.TryGetValue("MerchantName", out DocumentField merchantNameField))
            {
                if (merchantNameField.FieldType == DocumentFieldType.String)
                {
                    string merchantName = merchantNameField.ValueString;

                    sb.AppendLine($"Merchant Name: '{merchantName}', with confidence {merchantNameField.Confidence}");
                }
            }

            if (receipt.Fields.TryGetValue("TransactionDate", out DocumentField transactionDateField))
            {
                if (transactionDateField.FieldType == DocumentFieldType.Date)
                {
                    DateTimeOffset? transactionDate = transactionDateField.ValueDate;

                    sb.AppendLine($"Transaction Date: '{transactionDate}', with confidence {transactionDateField.Confidence}");
                }
            }

            if (receipt.Fields.TryGetValue("Items", out DocumentField itemsField))
            {
                if (itemsField.FieldType == DocumentFieldType.List)
                {
                    foreach (DocumentField itemField in itemsField.ValueList)
                    {
                        sb.AppendLine("Item:");

                        if (itemField.FieldType == DocumentFieldType.Dictionary)
                        {
                            IReadOnlyDictionary<string, DocumentField> itemFields = itemField.ValueDictionary;

                            if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField))
                            {
                                if (itemDescriptionField.FieldType == DocumentFieldType.String)
                                {
                                    string itemDescription = itemDescriptionField.ValueString;

                                    sb.AppendLine($"  Description: '{itemDescription}', with confidence {itemDescriptionField.Confidence}");
                                }
                            }

                            if (itemFields.TryGetValue("TotalPrice", out DocumentField itemTotalPriceField))
                            {
                                if (itemTotalPriceField.FieldType == DocumentFieldType.Currency)
                                {
                                    double? itemTotalPrice = itemTotalPriceField.ValueCurrency.Amount;

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
                    double total = totalField.ValueCurrency.Amount;

                    sb.AppendLine($"Total: '{total}', with confidence '{totalField.Confidence}'");
                }
            }
        }
        return sb.ToString();
    }
}
