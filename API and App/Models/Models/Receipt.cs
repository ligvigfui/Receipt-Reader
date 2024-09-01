namespace RR.Common.Models;

public class Receipt()
{
    public Vendor Vendor { get; set; }
    public List<ReceiptItem> Items { get; set; } = [];
    public long Total => Items.Sum(i => i.Price);
    public string? ReceiptId { get; set; }
    public DateTime? DateTime { get; set; }
}
