namespace RR.Data.Interfaces;

public interface IProductRepository
{
    Task<ProductDBO> GetOrCreateProductWithAliasAsync(ReceiptItem receiptItem, string? receiptLanguage, UserDBO user, UserGroupDBO? groupId);
    Task<List<ProductDBO>> GetProductsAsync(SortPageFilter<Product> sortPageFilter, int userShortId);
}
