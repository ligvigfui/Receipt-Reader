namespace RR.Data.DataBaseObjects;

[Tables(nameof(ProductAliasDBO))]
[PrimaryKey(nameof(Language), nameof(Name))]
public class ProductAliasDBO : AbstractPublicOwnable
{
    public required string Name { get; set; }
    [StringLength(5)]
    public required string Language { get; set; }
    public int ProductId { get; set; }
    public virtual ProductDBO Product { get; set; }
}
