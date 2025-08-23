namespace RR.Data.DataBaseObjects;

[Tables(nameof(ProductCategoryDBO))]
public class ProductCategoryDBO
{
    public int ProductId { get; set; }
    public virtual ProductDBO Product { get; set; }
    public int CategoryId { get; set; }
    public virtual CategoryDBO Category { get; set; }
}