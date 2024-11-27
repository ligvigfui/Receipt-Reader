namespace RR.Common.Models;

public class ReceiptItem
{
    public required string Name { get; set; }
    public int Quantity { get; set; } = 1;
    public required int PricePerQuantity { get; set; }
    public int Price => Quantity * PricePerQuantity;
}