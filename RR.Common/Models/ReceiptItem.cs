namespace RR.Common.Models;

public class ReceiptItem
{
    public string? OriginalRecognizedName { get; set; }
    public required string Name { get; set; }
    public float Quantity { get; set; }
    public Measurement Measurement { get; set; }
    public required float PricePerQuantity { get; set; }
    public float Price => Quantity * PricePerQuantity;
}