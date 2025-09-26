namespace RR.Common.Models;

public class Vendor
{
    public VendorHQ? VendorHQ { get; set; }
    public required string Name { get; set; }
    public Address? Address { get; set; }
}
