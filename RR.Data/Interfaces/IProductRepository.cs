namespace RR.Data.Interfaces;

public interface IProductRepository
{
    Task<ProductDBO> CreateProductAsync(Product product);
    Task<ProductDBO?> GetExactProductWithAliasAsync(string itemName, string language, int userShortId);
    Task<ProductDBO?> GetProductAsync(Product? product, int userShortId);
    Task<List<ProductDBO>> GetProductsAsync(SortPageFilter<Product> sortPageFilter, int userShortId);
    Task<List<ProductDBO>> GetProductsWithIdsAsync(IEnumerable<Product?> products, int userShortId, int? groupId);
}
