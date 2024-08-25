namespace RR.Common.Models;

public class ReceiptItem
{
    public string Name { get; set; }
    public int Quantity { get; set; } = 1;
    public int PricePerQuantity { get; set; }
    public int Price => Quantity * PricePerQuantity;
}