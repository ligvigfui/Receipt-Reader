namespace RR.Common.Models;

public class Product : IOwnable
{
    public string? Name { get; set; }
    public int? Id { get; set; }
    public int? GroupId { get; set; }
    public bool? IsPublic { get; set; }
    public string? Description { get; set; }
    public float? Quantity { get; set; }
    public Measurement? Measurement { get; set; }
    public string? ImageUrl { get; set; }
    public string ToIdAndNameString() => $"{{ Id: {Id}, Name: {Name} }}";
}
