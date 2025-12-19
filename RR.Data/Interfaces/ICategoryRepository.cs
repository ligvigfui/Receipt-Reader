
namespace RR.Data.Interfaces;

public interface ICategoryRepository
{
    Task<CategoryDBO> CreateCategory(Category category, int userShortId);
    Task<CategoryDBO> GetCategory(MinimalCategory category, uint depth = 1);
}
