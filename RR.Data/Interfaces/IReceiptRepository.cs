namespace RR.Data.Interfaces;

public interface IReceiptRepository
{
    Task<ReceiptDBO> CreateReceipt(ReceiptDBO receiptDBO);
}
