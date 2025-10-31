namespace RR.Data.Repository;

public class ProductAliasRepository(
    ApplicationDbContext context
) : IProductAliasRepository
{
    public async Task<List<ProductAliasDBO>> GetProductAliasesWithNamesAsync(IEnumerable<string> productNames, string language, int userShortId, int? groupId) =>
        await context.ProductAliases
            .WhereGroupCanRead(userShortId, groupId)
            .Where(pa => productNames.Contains(pa.Name) && pa.Language == language)
            .ToListAsync();

    public async Task<ProductAliasDBO?> GetProductAliasAsync(string? productName, string language, int userShortId) => productName is null ? null :
        await context.ProductAliases.WhereCanRead(userShortId).FirstOrDefaultAsync(pa => pa.Name == productName && pa.Language == language);

    public async Task<ProductAliasDBO> CreateProductAliasAsync(ProductAliasDBO productAliasDBO)
    {
        context.ProductAliases.Add(productAliasDBO);
        await context.SaveChangesAsync();
        return productAliasDBO;
    }
}
