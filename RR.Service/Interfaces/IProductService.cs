namespace RR.Service.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetProductsAsync(SortPageFilter<Product> sortPageFilter);
}
