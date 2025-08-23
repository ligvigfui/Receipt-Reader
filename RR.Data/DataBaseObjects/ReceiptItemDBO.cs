namespace RR.Data.DataBaseObjects;

[Tables(nameof(ReceiptItemDBO))]
public class ReceiptItemDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ReceiptId { get; set; }
    public virtual ReceiptDBO ReceiptDBO { get; set; }
    public int ProductId { get; set; }
    public virtual ProductDBO Product { get; set; }
    public float Quantity { get; set; }
    public Measurement Measurement { get; set; }
    public float PricePerQuantity { get; set; }
    public float Price => Quantity * PricePerQuantity;

    public static implicit operator ReceiptItem(ReceiptItemDBO receiptItemDBO) => new()
    {
        Name = receiptItemDBO.Product.Name,
        Quantity = receiptItemDBO.Quantity,
        Measurement = receiptItemDBO.Measurement,
        PricePerQuantity = receiptItemDBO.PricePerQuantity,
    };

    public static implicit operator ReceiptItemDBO(ReceiptItem receiptItem) => new()
    {
        Product = new ProductDBO { Name = receiptItem.Name },
        Quantity = receiptItem.Quantity,
        Measurement = receiptItem.Measurement,
        PricePerQuantity = receiptItem.PricePerQuantity,
    };
}
