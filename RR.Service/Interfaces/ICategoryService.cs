namespace RR.Service.Interfaces;

public interface ICategoryService
{
    Task<MinimalCategory> CreateCategory(Category category);
    Task<MinimalCategory> GetCategory(MinimalCategory category, uint depth = 1);
}
