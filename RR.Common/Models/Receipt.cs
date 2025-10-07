namespace RR.Common.Models;

public class Receipt
{
    public int? Id { get; set; }
    public int? GroupId { get; set; }
    public Vendor Vendor { get; set; }
    public List<ReceiptItem> Items { get; set; } = [];
    public double Total => Items.Sum(i => i.Price);
    public DateTime? TransactionDateTime { get; set; }
}
