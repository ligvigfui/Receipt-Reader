
namespace RR.Data.Interfaces;

public interface IReceiptItemRepository
{
    Task<ReceiptItemDBO> CreateReceiptItemAsync(ReceiptItem receiptItem, string language, int userShortId, int? groupId);
}
