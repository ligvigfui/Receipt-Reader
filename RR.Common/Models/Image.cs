namespace RR.Common.Models;

public class Image : IDisposable, IOwnable
{
    public int? Id { get; set; }
    public int? GroupId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public Stream Stream { get; set; }
    public bool IsPublic { get; set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
