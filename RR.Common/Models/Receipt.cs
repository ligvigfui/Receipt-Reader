namespace RR.Common.Models;

public class Receipt
{
    public string? GroupName { get; set; }
    public Vendor Vendor { get; set; }
    public List<ReceiptItem> Items { get; set; } = [];
    public double Total => Items.Sum(i => i.Price);
    public string? ReceiptId { get; set; }
    public DateTime? DateTime { get; set; }
}
