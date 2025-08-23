
namespace RR.Data.Interfaces;

public interface IProductRepository
{
    Task<ProductDBO> GetOrCreateProductAsync(string name);
}
