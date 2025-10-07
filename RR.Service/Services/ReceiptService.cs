namespace RR.Service.Services;

public class ReceiptService(
    IProductRepository productRepository,
    IReceiptRepository receiptRepository,
    ISecurityService securityService,
    IVendorRepository vendorRepository,
    ApplicationDbContext context
) : IReceiptService
{
    public async Task<ReceiptDBO> CreateReceiptAsync(Receipt receipt, string? groupName = null)
    {
        var userDBO = await securityService.GetUserAsync();
        var newReceipt = new ReceiptDBO
        {
            UserId = userDBO.Id,
            GroupId = groupName is null ? null : context.Groups.FirstOrDefault(g => g.Name == groupName)?.Id,
            VendorId = await vendorRepository.CreateVendorAsync(receipt.Vendor),
            Items = [.. await Task.WhenAll(
                receipt.Items.Select(async i =>
                    new ReceiptItemDBO
                    {
                        OriginalRecognizedName = i.OriginalRecognizedName,
                        Product = await productRepository.GetOrCreateProductAsync(i.Name),
                        Quantity = i.Quantity,
                        Measurement = i.Measurement,
                        PricePerQuantity = i.PricePerQuantity,
                    }
                )
            )],
            TransactionDateTime = receipt.TransactionDateTime,
        };
        newReceipt = await receiptRepository.CreateReceipt(newReceipt);
        return newReceipt;
    }

    //public async Task<Receipt> UpdateReceiptAsync(Receipt receipt)
    //{

    //}
}
