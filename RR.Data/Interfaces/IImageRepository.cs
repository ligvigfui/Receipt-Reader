namespace RR.Data.Interfaces;

public interface IImageRepository
{
    Task<ImageDBO> CreateImageAsync(ImageDBO imageDBO);
    Task<ImageDBO?> GetImageAsync(string fileName, int? imageId, int userShortId);
}
