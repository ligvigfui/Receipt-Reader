namespace RR.Common.Models;

public class Vendor : IOwnable
{
    public int? Id { get; set; }
    public int? GroupId { get; set; }
    public VendorHQ? VendorHQ { get; set; }
    public required string Name { get; set; }
    public Address? Address { get; set; }
}
