namespace RR.Data.Interfaces;

public interface IReceiptItemRepository
{
    Task<ReceiptItemDBO> CreateReceiptItemAsync(ReceiptItem receiptItem, ushort languageId, int userShortId, int? groupId);
    Task<List<ReceiptItemDBO>> PreProcessReceiptItemsAsync(IEnumerable<ReceiptItem> receiptItems, ushort languageId, UserDBO user, int? groupId);
    Task<List<ReceiptItemValidated>> ValidateItems(List<ReceiptItemDBO> items);
}
