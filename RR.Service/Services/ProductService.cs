namespace RR.Service.Services;

public class ProductService(
    IProductRepository productRepository,
    ISecurityService securityService
) : IProductService
{
    public async Task<List<Product>> GetProductsAsync(SortPageFilter<Product> sortPageFilter) =>
        [.. (await productRepository.GetProductsAsync(sortPageFilter, (await securityService.GetUserAsync()).ShortId)).Select(x => (Product)x)];
}