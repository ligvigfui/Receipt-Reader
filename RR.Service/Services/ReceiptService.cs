namespace RR.Service.Services;

public class ReceiptService(
    IReceiptRepository receiptRepository,
    IReceiptItemRepository receiptItemRepository,
    ISecurityService securityService,
    IVendorRepository vendorRepository
) : IReceiptService
{
    public async Task<Receipt> CreateReceiptAsync(Receipt receipt)
    {
        var userDBO = await securityService.GetUserAsync();
        var userGroup = await securityService.EnsureCanEditOwn(receipt);
        var language = userDBO.GetLanguage(receipt.Language);
        var receiptItems = new List<ReceiptItemDBO>();
        foreach (var receiptItem in receipt.Items)
        {
            receiptItems.Add(await receiptItemRepository.CreateReceiptItemAsync(receiptItem, language, userDBO.ShortId, userGroup?.GroupId));
        } 
        var vendorDBO = await vendorRepository.GetVendorAsync(receipt.Vendor, userDBO.ShortId) ??
            await vendorRepository.CreateVendorAsync(new VendorDBO
            {
                Name = receipt.Vendor.Name,
                UserShortId = userDBO.ShortId,
                GroupId = userGroup?.GroupId
            });
        var newReceipt = new ReceiptDBO
        {
            UserShortId = userDBO.ShortId,
            GroupId = userGroup?.GroupId,
            VendorId = await vendorRepository.CreateVendorAsync(receipt.Vendor),
            Items = receiptItems,
            TransactionDateTime = receipt.TransactionDateTime,
        };
        newReceipt = await receiptRepository.CreateReceipt(newReceipt);
        return newReceipt;
    }

    //public async Task<Receipt> UpdateReceiptAsync(Receipt receipt)
    //{

    //}
}
