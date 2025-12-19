namespace RR.Common.Models.Category;

public class Category : MinimalCategory
{
    public virtual MinimalCategory? ParentCategory { get; set; }
    public virtual ICollection<MinimalCategory>? SubCategories { get; set; }
}
