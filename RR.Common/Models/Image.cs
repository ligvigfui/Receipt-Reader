namespace RR.Common.Models;

public class Image : IDisposable
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public Stream Stream { get; set; }
    public bool IsPublic { get; set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
