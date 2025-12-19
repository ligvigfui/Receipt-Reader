namespace RR.Common.Models;

public class Image : IOwnable
{
    public int? Id { get; set; }
    public int? GroupId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public bool IsPublic { get; set; }
}