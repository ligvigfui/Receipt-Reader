namespace RR.Data.DataBaseObjects;

[Table(nameof(ReceiptItemDBO))]
public class ReceiptItemDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ReceiptId { get; set; }
    public virtual ReceiptDBO ReceiptDBO { get; set; }
    public string Name { get; set; }
    public float Quantity { get; set; }
    public Measurement Measurement { get; set; }
    public float PricePerQuantity { get; set; }
    public float Price => Quantity * PricePerQuantity;

    public static implicit operator ReceiptItem(ReceiptItemDBO receiptItemDBO) => new()
    {
        Name = receiptItemDBO.Name,
        Quantity = receiptItemDBO.Quantity,
        Measurement = receiptItemDBO.Measurement,
        PricePerQuantity = receiptItemDBO.PricePerQuantity,
    };

    public static implicit operator ReceiptItemDBO(ReceiptItem receiptItem) => new()
    {
        Name = receiptItem.Name,
        Quantity = receiptItem.Quantity,
        Measurement = receiptItem.Measurement,
        PricePerQuantity = receiptItem.PricePerQuantity
    };
}
