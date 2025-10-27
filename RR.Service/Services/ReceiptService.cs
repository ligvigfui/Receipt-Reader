namespace RR.Service.Services;

public class ReceiptService(
    IReceiptRepository receiptRepository,
    IReceiptItemRepository receiptItemRepository,
    IGroupRepository groupRepository,
    ISecurityService securityService,
    IVendorRepository vendorRepository
) : IReceiptService
{
    public async Task<Receipt> CreateReceiptAsync(Receipt receipt)
    {
        if (receipt.Id is not null)
            throw new BadRequestException("Receipt to be created cannot already have an Id.");
        var userDBO = await securityService.GetUserAsync();
        var userGroup = await groupRepository.EnsureCanEditOwn(receipt.GroupId, userDBO.ShortId);
        var language = userDBO.GetLanguage(receipt.Language);
        var receiptItems = new List<ReceiptItemDBO>();
        foreach (var receiptItem in receipt.Items)
        {
            receiptItems.Add(await receiptItemRepository.CreateReceiptItemAsync(receiptItem, language, userDBO.ShortId, userGroup?.GroupId));
        }
        var vendorDBO = await vendorRepository.GetOrCreateVendorAsync(new VendorDBO(receipt.Vendor, userDBO.ShortId));
        var newReceipt = new ReceiptDBO
        {
            UserShortId = userDBO.ShortId,
            GroupId = userGroup?.GroupId,
            VendorId = vendorDBO.Id,
            Items = receiptItems,
            TransactionDateTime = receipt.TransactionDateTime,
        };
        newReceipt = await receiptRepository.CreateReceipt(newReceipt);
        return newReceipt;
    }
}
