namespace RR.Service.Services;

public class ReceiptService(
    IProductRepository productRepository,
    IReceiptRepository receiptRepository,
    ISecurityService securityService,
    IVendorRepository vendorRepository
) : IReceiptService
{
    public async Task<ReceiptDBO> CreateReceiptAsync(Receipt receipt)
    {
        var userDBO = await securityService.GetUserAsync();
        var newReceipt = new ReceiptDBO
        {
            UserId = userDBO.Id,
            VendorId = await vendorRepository.CreateVendorAsync(receipt.Vendor),
            Items = [.. await Task.WhenAll(
                receipt.Items.Select(async i =>
                    new ReceiptItemDBO
                    {
                        Product = await productRepository.GetOrCreateProductAsync(i.Name),
                        Quantity = i.Quantity,
                        Measurement = i.Measurement,
                        PricePerQuantity = i.PricePerQuantity,
                    }
                )
            )],
            ReceiptId = receipt.ReceiptId,
            DateTime = receipt.DateTime,
        };
        newReceipt = await receiptRepository.CreateReceipt(newReceipt);
        return newReceipt;
    }
}
