namespace RR.Data.DataBaseObjects;

[Tables(nameof(ProductCategoryDBO))]
[PrimaryKey(nameof(ProductId), nameof(CategoryId))]
public class ProductCategoryDBO
{
    public int ProductId { get; set; }
    public virtual ProductDBO Product { get; set; }
    public int CategoryId { get; set; }
    public virtual CategoryDBO Category { get; set; }
}