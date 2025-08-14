namespace RR.Service.Services;

public class ReceiptService(
    IReceiptRepository receiptRepository,
    ISecurityService securityService
) : IReceiptService
{
    public async Task<ReceiptDBO> CreateReceiptAsync(Receipt receipt)
    {
        var userDBO = await securityService.GetUserAsync();
        var newReceipt = new ReceiptDBO(receipt, userDBO.Id);
        await receiptRepository.CreateReceipt(newReceipt);
        return newReceipt;
    }
}
