namespace RR.Common.Models;

public class VendorHQ : IOwnable
{
    public int? Id { get; set; }
    public int? GroupId { get; set; }
    public required string Name { get; set; }
    public Address? Address { get; set; }
    public string? TaxNumber { get; set; }
}

public class VendorHQValidated : VendorHQ
{
    public Dictionary<string, string>? ValidationMessages { get; set; }
}