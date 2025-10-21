namespace RR.Data.Repository;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<ProductDBO> GetOrCreateProductWithAliasAsync(ReceiptItem receiptItem, string receiptLanguage, UserDBO user, UserGroupDBO? userGroup)
    {
        var language = receiptLanguage ?? user.DefaultLanguage ?? throw new BadRequestException("Language must be specified either in receipt or user profile.");
        var existingProduct = await GetProductWithAliasAsync(receiptItem, language, user.ShortId);
        if (existingProduct != null)
            return existingProduct;
        var product = new ProductDBO
        {
            UserShortId = user.ShortId,
            GroupId = userGroup?.GroupId,
            IsPublic = receiptItem.Product?.IsPublic ?? userGroup?.Group.AreItemsDefaultPublic ?? userGroup?.AreItemsDefaultPublic ?? user.AreItemsDefaultPublic,
            Name = receiptItem.Product?.Name ?? receiptItem.Name,
            Description = receiptItem.Product?.Description,
            Quantity = receiptItem.Product?.Quantity,
            Measurement = receiptItem.Product?.Measurement,
            ImageUrl = receiptItem.Product?.ImageUrl,
            Aliases =
            [
                new ProductAliasDBO
                {
                    Name = receiptItem.Name,
                    Language = language,
                    UserShortId = user.ShortId,
                    GroupId = userGroup?.GroupId,
                }
            ]
        };
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return product;
    }
    public async Task<ProductDBO?> GetProductWithAliasAsync(ReceiptItem receiptItem, string language, int userShortId)
    {
        var alias = await context.ProductAliases
            .Where(alias =>
                alias.Name == receiptItem.Name &&
                alias.Language == language)
            .WhereCanRead(userShortId)
            .FirstOrDefaultAsync();
        if (alias != null)
            return await context.Products.Include(x => x.Aliases.Where(a => a.Name == receiptItem.Name)).FirstAsync(product => product.Id == alias.ProductId);
        var product = await context.Products.FirstOrDefaultAsync(product => product.Name == receiptItem.Name);
        return product;
    }

    public async Task<List<ProductDBO>> GetProductsAsync(SortPageFilter<Product> sortPageFilter, int userShortId) =>
        await context.Products.WhereCanRead(userShortId).GetPagedAsync(sortPageFilter);
}
