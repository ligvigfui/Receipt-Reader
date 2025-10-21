namespace RR.Service.Services;

public class ReceiptService(
    IProductRepository productRepository,
    IReceiptRepository receiptRepository,
    ISecurityService securityService,
    IVendorRepository vendorRepository
) : IReceiptService
{
    public async Task<Receipt> CreateReceiptAsync(Receipt receipt)
    {
        var userDBO = await securityService.GetUserAsync();
        var userGroup = await securityService.EnsureCanEditOwn(receipt);
        var newReceipt = new ReceiptDBO
        {
            UserShortId = userDBO.ShortId,
            GroupId = userGroup?.GroupId,
            VendorId = await vendorRepository.CreateVendorAsync(receipt.Vendor),
            Items = [.. await Task.WhenAll(
                receipt.Items.Select(async i =>
                    new ReceiptItemDBO
                    {
                        Product = await productRepository.GetOrCreateProductWithAliasAsync(
                            i,
                            receipt.Language,
                            userDBO,
                            userGroup
                        ),
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
