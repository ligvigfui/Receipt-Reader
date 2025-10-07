namespace RR.Data.DataBaseObjects;

[Tables(nameof(ReceiptDBO))]
public class ReceiptDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string UserId { get; set; }
    public virtual UserDBO User { get; set; }
    public int? GroupId { get; set; }
    public virtual GroupDBO? Group { get; set; }
    public int VendorId { get; set; }
    public virtual VendorDBO Vendor { get; set; }
    public virtual List<ReceiptItemDBO> Items { get; set; }
    public double Total => Items.Sum(i => i.Price);
    public DateTime? TransactionDateTime { get; set; }
    public int? ImageId { get; set; }
    public virtual ImageDBO? Image { get; set; }

    public static implicit operator Receipt(ReceiptDBO receiptDBO) => new()
    {
        Id = receiptDBO.Id,
        GroupId = receiptDBO.GroupId,
        Vendor = receiptDBO.Vendor,
        Items = [.. receiptDBO.Items.Select(i => (ReceiptItem)i)],
        TransactionDateTime = receiptDBO.TransactionDateTime,
    };
}
