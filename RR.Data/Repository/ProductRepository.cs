namespace RR.Data.Repository;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<ProductDBO> GetOrCreateProductAsync(string name)
    {
        var product = await context.Products.FirstOrDefaultAsync(product => product.Name == name);
        if (product != null)
            return product;
        product = new ProductDBO
        {
            Name = name
        };
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async 
}
