
namespace RR.Service.Interfaces;

public interface IImageService
{
    Task<Uri> CreateImageAndGetURIAsync(IFormFile image, bool isPublic, int? groupId);
    Task<Image> CreateImageAsync(IFormFile image, bool isPublic, int? groupId);
    Task<ImageDBO> CreateImageDBOAsync(IFormFile image, bool isPublic, int? groupId);
    Task<Uri> GetImageURI(string fileName, int? fileId, int? userShortId = null);
}
