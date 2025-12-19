namespace RR.Data.Interfaces;

public interface IProductAliasRepository
{
    Task<ProductAliasDBO> CreateProductAliasAsync(ProductAliasDBO productAliasDBO);
    Task<ProductAliasDBO?> GetProductAliasAsync(string? productName, ushort languageId, int userShortId);
    Task<List<ProductAliasDBO>> GetProductAliasesWithNamesAsync(IEnumerable<string> productNames, ushort languageId, int userShortId, int? groupId);
}
