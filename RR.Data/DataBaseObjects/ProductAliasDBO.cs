namespace RR.Data.DataBaseObjects;

[Tables(nameof(ProductAliasDBO))]
public class ProductAliasDBO : AbstractPublicOwnable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    [StringLength(5)]
    public required string Language { get; set; }
    public int ProductId { get; set; }
    public virtual ProductDBO Product { get; set; }
    public string ToIdAndNameString() => $"{{ Id: {Id}, Name: {Name} }}";
}
