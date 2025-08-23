namespace RR.Data.DataBaseObjects;

[Tables(nameof(CategoryDBO))]
public class CategoryDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? ParentCategoryId { get; set; }
    public virtual CategoryDBO? ParentCategory { get; set; }
    public virtual ICollection<CategoryDBO> SubCategories { get; set; }
    public virtual ICollection<ProductDBO> Products { get; set; }
    internal virtual ICollection<ProductCategoryDBO> ProductCategories { get; set; }
}