

namespace RR.Data.Interfaces;

public interface IProductAliasRepository
{
    Task<ProductAliasDBO> CreateProductAliasAsync(ProductAliasDBO productAliasDBO);
    Task<ProductAliasDBO?> GetProductAliasAsync(string? productName, string language, int userShortId);
    Task<List<ProductAliasDBO>> GetProductAliasesWithNamesAsync(IEnumerable<string> productNames, string language, int userShortId, int? groupId);
}
