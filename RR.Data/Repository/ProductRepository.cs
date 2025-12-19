namespace RR.Data.Repository;

public class ProductRepository(
    IMeasurementRepository measurementRepository,
    ApplicationDbContext context
) : IProductRepository
{
    public async Task<ProductDBO> CreateProductAsync(Product product)
    {
        var measurementDBO = await measurementRepository.GetMeasurement(product.Measurement);
        var productDBO = new ProductDBO
        {
            Name = product.Name!,
            Description = product.Description,
            Quantity = product.Quantity,
            MeasurementId = measurementDBO?.Id,
            ImageUrl = product.ImageUrl,
        };
        await context.Products.AddAsync(productDBO);
        await context.SaveChangesAsync();
        return productDBO;
    }

    public async Task<ProductDBO?> GetExactProductWithAliasAsync(string itemName, byte languageId, int userShortId)
    {
        var alias = await context.ProductAliases
            .Where(alias =>
                alias.Name == itemName &&
                alias.LanguageId == languageId)
            .WhereCanRead(userShortId)
            .FirstOrDefaultAsync();
        if (alias != null)
            return await context.Products.Include(x => x.Aliases.Where(a => a.Name == itemName)).FirstAsync(product => product.Id == alias.ProductId);
        var product = await context.Products.FirstOrDefaultAsync(product => product.Name == itemName);
        return product;
    }

    public async Task<List<ProductDBO>> GetProductsWithIdsAsync(IEnumerable<Product> products, int userShortId, int? groupId)
    {
        var productIds = products.Where(p => p.Id is not null).Select(p => p.Id).Distinct().ToList();
        return await context.Products.WhereGroupCanRead(userShortId, groupId).Where(p => productIds.Contains(p.Id)).ToListAsync();
    }

    public async Task<ProductDBO?> GetProductAsync(Product? product, int userShortId) => product is null ? null :
        await context.Products.WhereCanRead(userShortId).FirstOrDefaultAsync(p => p.Name == product.Name && product.Id == p.Id);


    public async Task<List<ProductDBO>> GetProductsAsync(SortPageFilter<Product> sortPageFilter, int userShortId) =>
        await context.Products.WhereCanRead(userShortId).Include(p => p.Measurement).GetPagedAsync(sortPageFilter);
}
