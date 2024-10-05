namespace RR.Data.DataBaseObjects;

[Table(nameof(ReceiptDBO))]
public class ReceiptDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string UserId { get; set; }
    public virtual UserDBO User { get; set; }
    public int VendorId { get; set; }
    public virtual VendorDBO Vendor { get; set; }
    public virtual List<ReceiptItemDBO> Items { get; set; }
    public long Total => Items.Sum(i => i.Price);
    public string? ReceiptId { get; set; }
    public DateTime? DateTime { get; set; }

    public static implicit operator Receipt(ReceiptDBO receiptDBO) => new()
    {
        Vendor = receiptDBO.Vendor,
        Items = receiptDBO.Items.Select(i => (ReceiptItem)i).ToList(),
        ReceiptId = receiptDBO.ReceiptId,
        DateTime = receiptDBO.DateTime,
    };

    public ReceiptDBO() { }
    public ReceiptDBO(Receipt receipt, string userId)
    {
        UserId = userId;
        Vendor = receipt.Vendor;
        Items = receipt.Items.Select(i => (ReceiptItemDBO)i).ToList();
        ReceiptId = receipt.ReceiptId;
        DateTime = receipt.DateTime;
    }
}
