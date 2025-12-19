namespace RR.Data.Repository;

public class CategoryRepository(
    ApplicationDbContext context    
) : ICategoryRepository
{
    public async Task<CategoryDBO> CreateCategory(Category category, int userShortId)
    {
        if (category.ParentCategory?.Name is null)
            throw new InvalidOperationException($"Parent category name must be provided.");
        var parentCategory = await context.Categories.WhereCanRead(userShortId).FirstOrDefaultAsync(c => 
            c.Name == category.ParentCategory.Name &&
            (category.ParentCategory.Id == null ||
             category.ParentCategory.Id == c.Id)) ??
            throw new InvalidOperationException($"Parent category with name {category.ParentCategory.Name} was not found.");
        if (await context.Categories.WhereCanRead(userShortId).AnyAsync(c =>
            c.Name == category.Name &&
            c.ParentCategoryId == parentCategory.Id))
            throw new InvalidOperationException($"Category with name {category.Name} already exists.");
        var newCategory = new CategoryDBO(category)
        {
            ParentCategoryId = parentCategory.Id,
        };
        context.Categories.Add(newCategory);
        await context.SaveChangesAsync();
        return newCategory;
    }

    public async Task<CategoryDBO> GetCategory(MinimalCategory category, uint depth = 1)
    {
        var query = context.Categories
            .Where(c => (category.Id == null || c.Id == category.Id) && c.Name == category.Name)
            .Include(c => c.SubCategories);
        for (int i = 1; i < depth; i++)
            query = query.ThenInclude(c => c.SubCategories);
        
        return await query.SingleOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Category with Id {category.Id} and Name {category.Name} not found.");
    }
}
