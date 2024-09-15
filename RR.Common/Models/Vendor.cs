namespace RR.Common.Models;

public class Vendor
{
    public VendorHQ? VendorHQ { get; set; }
    public string? Name { get; set; }
    public Address Address { get; set; }
    public string? TaxNumber { get; set; }
}
