namespace RR.Data.DataBaseObjects;

[Tables(nameof(ProductDBO))]
public class ProductDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int? UsertId { get; set; }
    public virtual UserDBO? User { get; set; }
    public int? GroupId { get; set; }
    public virtual GroupDBO? Group { get; set; }
    public bool IsPublic { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public float? Quantity { get; set; }
    public Measurement? Measurement { get; set; }
    public string? ImageUrl { get; set; }
    public virtual ICollection<ProductAliasDBO> Aliases { get; set; }
    public virtual ICollection<ReceiptItemDBO> ReceiptItems { get; set; }
    public virtual ICollection<CategoryDBO> Categories { get; set; }
    internal virtual ICollection<ProductCategoryDBO> ProductCategories { get; set; }
}