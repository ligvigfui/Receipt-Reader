namespace RR.Common.Models;

public class Receipt : IOwnable
{
    public int? Id { get; set; }
    public int? GroupId { get; set; }
    public string? Language { get; set; }
    public Vendor? Vendor { get; set; }
    public IEnumerable<ReceiptItem> Items { get; set; } = [];
    public double Total => Items.Sum(i => i.Price);
    public DateTime? TransactionDateTime { get; set; }
}

public class ReceiptValidated : Receipt
{
    public Dictionary<string, string>? ValidationMessages { get; set; }
}