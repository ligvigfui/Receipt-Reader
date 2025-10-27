namespace RR.Data.DataBaseObjects;

[Tables(nameof(ProductDBO))]
public class ProductDBO : AbstractPublicOwnable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public float? Quantity { get; set; }
    public int? MeasurementId { get; set; }
    public virtual MeasurementDBO? Measurement { get; set; }
    public string? ImageUrl { get; set; }
    public virtual ICollection<ProductAliasDBO> Aliases { get; set; }
    public virtual ICollection<ReceiptItemDBO> ReceiptItems { get; set; }
    public virtual ICollection<CategoryDBO> Categories { get; set; }
    internal virtual ICollection<ProductCategoryDBO> ProductCategories { get; set; }
    public static implicit operator Product(ProductDBO productDBO) => new()
    {
        Id = productDBO.Id,
        GroupId = productDBO.GroupId,
        IsPublic = productDBO.IsPublic,
        Name = productDBO.Name,
        Description = productDBO.Description,
        Quantity = productDBO.Quantity,
        Measurement = productDBO.Measurement,
        ImageUrl = productDBO.ImageUrl,
    };
    public string ToIdAndNameString() => $"{{ Id: {Id}, Name: {Name} }}";
}