namespace RR.Data.Repository;

public class ProductAliasRepository(
    ApplicationDbContext context
) : IProductAliasRepository
{
    public async Task<List<ProductAliasDBO>> GetProductAliasesWithNamesAsync(IEnumerable<string> productNames, ushort languageId, int userShortId, int? groupId) =>
        await context.ProductAliases
            .WhereGroupCanRead(userShortId, groupId)
            .Where(pa => productNames.Contains(pa.Name) && pa.LanguageId == languageId)
            .Include(pa => pa.Product)
            .ToListAsync();

    public async Task<ProductAliasDBO?> GetProductAliasAsync(string? productName, ushort languageId, int userShortId) => productName is null ? null :
        await context.ProductAliases.WhereCanRead(userShortId).FirstOrDefaultAsync(pa => pa.Name == productName && pa.LanguageId == languageId);

    public async Task<ProductAliasDBO> CreateProductAliasAsync(ProductAliasDBO productAliasDBO)
    {
        context.ProductAliases.Add(productAliasDBO);
        await context.SaveChangesAsync();
        return productAliasDBO;
    }
}
