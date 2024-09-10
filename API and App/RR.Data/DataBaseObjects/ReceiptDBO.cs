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
    string ItemsJson
    {
        get => JsonSerializer.Serialize(Items);
        set => Items = JsonSerializer.Deserialize<List<ReceiptItem>>(value) ?? [];
    }
    [NotMapped]
    public virtual List<ReceiptItem> Items { get; set; } = [];
    public long Total => Items.Sum(i => i.Price);
    public string? ReceiptId { get; set; }
    public DateTime? DateTime { get; set; }

    public static implicit operator Receipt(ReceiptDBO receiptDBO) => new()
    {
        Vendor = receiptDBO.Vendor,
        Items = receiptDBO.Items,
        ReceiptId = receiptDBO.ReceiptId,
        DateTime = receiptDBO.DateTime,
    };

    public ReceiptDBO() { }
    public ReceiptDBO(Receipt receipt, string userId)
    {
        UserId = userId;
        Vendor = receipt.Vendor;
        Items = receipt.Items;
        ReceiptId = receipt.ReceiptId;
        DateTime = receipt.DateTime;
    }
}
