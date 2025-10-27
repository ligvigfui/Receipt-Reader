namespace RR.Data.DataBaseObjects;

[Tables(nameof(ReceiptItemDBO))]
public class ReceiptItemDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ReceiptId { get; set; }
    public virtual ReceiptDBO Receipt { get; set; }
    public int? ProductAliasId { get; set; }
    public virtual ProductAliasDBO? ProductAlias { get; set; }
    public int ProductId { get; set; }
    public virtual ProductDBO Product { get; set; }
    public float Quantity { get; set; }
    public int MeasurementId { get; set; }
    public virtual MeasurementDBO Measurement { get; set; }
    public float PricePerQuantity { get; set; }
    public float Price => Quantity * PricePerQuantity;

    public static implicit operator ReceiptItem(ReceiptItemDBO receiptItemDBO) => new()
    {
        Name = receiptItemDBO.ProductAlias?.Name,
        Product = receiptItemDBO.Product,
        Quantity = receiptItemDBO.Quantity,
        Measurement = receiptItemDBO.Measurement!,
        PricePerQuantity = receiptItemDBO.PricePerQuantity,
    };
}
