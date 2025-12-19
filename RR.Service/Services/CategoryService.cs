namespace RR.Service.Services;

public class CategoryService(
    ICategoryRepository categoryRepository,
    ISecurityService securityService
) : ICategoryService
{
    public async Task<MinimalCategory> CreateCategory(Category category)
    {
        return (await categoryRepository.CreateCategory(category, (await securityService.GetUserAsync()).ShortId)).ToCategory();
    }

    public async Task<MinimalCategory> GetCategory(MinimalCategory category, uint depth = 1)
    {
        return (await categoryRepository.GetCategory(category, depth)).ToCategory(depth);
    }

}
