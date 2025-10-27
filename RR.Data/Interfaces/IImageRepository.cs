
namespace RR.Data.Interfaces;

public interface IImageRepository
{
    Task<ImageDBO> CreateImageAsync(ImageDBO imageDBO);
    Task<ImageDBO?> GetImageBlobUrlAsync(string fileName, int userShortId);
}
