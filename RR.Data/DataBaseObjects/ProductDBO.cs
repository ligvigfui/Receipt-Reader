namespace RR.Data.DataBaseObjects;

[Tables(nameof(ProductDBO))]
public class ProductDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<ReceiptItemDBO> ReceiptItems { get; set; }
    public virtual ICollection<CategoryDBO> Categories { get; set; }
    internal virtual ICollection<ProductCategoryDBO> ProductCategories { get; set; }
}