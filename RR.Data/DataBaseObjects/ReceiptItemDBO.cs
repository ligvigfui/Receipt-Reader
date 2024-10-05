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
    public int Quantity { get; set; } = 1;
    public int PricePerQuantity { get; set; }
    public int Price => Quantity * PricePerQuantity;

    public static implicit operator ReceiptItem(ReceiptItemDBO receiptItemDBO) => new()
    {
        Name = receiptItemDBO.Name,
        Quantity = receiptItemDBO.Quantity,
        PricePerQuantity = receiptItemDBO.PricePerQuantity,
    };

    public static implicit operator ReceiptItemDBO(ReceiptItem receiptItem) => new()
    {
        Name = receiptItem.Name,
        Quantity = receiptItem.Quantity,
        PricePerQuantity = receiptItem.PricePerQuantity
    };
}
