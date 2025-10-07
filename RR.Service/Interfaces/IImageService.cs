

namespace RR.Service.Interfaces;

public interface IImageService
{
    Task<ImageDBO> CreateImageAsync(IFormFile image, bool isPublic, int? groupId);
    Task<Uri> GetImageURI(string fileName, int? groupId);
}
