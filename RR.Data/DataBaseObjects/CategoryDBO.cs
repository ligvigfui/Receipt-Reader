using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace RR.Data.DataBaseObjects;

[Tables(nameof(CategoryDBO))]
public class CategoryDBO : AbstractPublicOwnable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool CategoryDefinition { get; set; } = false;
    public int? ParentCategoryId { get; set; }
    public virtual CategoryDBO? ParentCategory { get; set; }
    public virtual ICollection<CategoryDBO> SubCategories { get; set; }
    public virtual ICollection<ProductDBO> Products { get; set; }
    internal virtual ICollection<ProductCategoryDBO> ProductCategories { get; set; }

    public CategoryDBO() { }
    [SetsRequiredMembers]
    public CategoryDBO(MinimalCategory minimalCategory)
    {
        Name = minimalCategory.Name;
        CategoryDefinition = minimalCategory.CategoryDefinition;
    }
    [SetsRequiredMembers]
    public CategoryDBO(Category category)
    {
        Name = category.Name;
        CategoryDefinition = category.CategoryDefinition;
        if (category.SubCategories is not null)
            SubCategories = [.. category.SubCategories.Select(sc => new CategoryDBO(sc))];
    }
    [SetsRequiredMembers]
    public CategoryDBO(string name, bool categoryDefinition, ICollection<CategoryDBO>? subCategories = null)
    {
        Name = name;
        if (subCategories is not null)
            SubCategories = subCategories;
        CategoryDefinition = categoryDefinition;
    }
    [SetsRequiredMembers]
    public CategoryDBO(string name, ICollection<CategoryDBO>? subCategories = null)
    {
        Name = name;
        if (subCategories is not null)
            SubCategories = subCategories;
    }

    public MinimalCategory ToCategory(uint depth = 1, bool initialCall = true)
    {
        return depth == 0
            ? ToMinimalCategory()
            : new Category
            {
                Id = Id,
                Name = Name,
                CategoryDefinition = CategoryDefinition,
                ParentCategory = initialCall ? ParentCategory?.ToMinimalCategory() : null,
                SubCategories = SubCategories?.Select(sc => sc.ToCategory(depth - 1, false)).ToList()
            };
    }

    public MinimalCategory ToMinimalCategory()
    {
        return new MinimalCategory
        {
            Id = Id,
            Name = Name,
            CategoryDefinition = CategoryDefinition
        };
    }
}