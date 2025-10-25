namespace RR.Common.Models;

public class ReceiptItem
{
    public string? Name { get; set; }
    public Product? Product { get; set; }
    public float Quantity { get; set; }
    public Measurement Measurement { get; set; }
    public required float PricePerQuantity { get; set; }
    public float Price => Quantity * PricePerQuantity;
}