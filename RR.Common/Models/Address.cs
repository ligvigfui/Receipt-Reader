namespace RR.Common.Models;

public class Address : IOwnable
{
    public int? Id { get; set; }
    public int? GroupId { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string City { get; set; }
    public string StreetAddress { get; set; }
    public string? Note { get; set; }
}
