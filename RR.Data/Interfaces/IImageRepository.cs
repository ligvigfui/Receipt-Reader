
namespace RR.Data.Interfaces;

public interface IImageRepository
{
    Task<ImageDBO> CreateImageAsync(ImageDBO imageDBO);
    Task<string?> GetImageBlobUrlAsync(string fileName, int userShortId, int? groupId);
}
