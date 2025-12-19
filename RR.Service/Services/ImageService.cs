using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RR.Service.Services;

public class ImageService(
    BlobServiceClient blobServiceClient,
    IOptions<AzureBlobStorageSettings> azureBlobStorageSettings,
    IImageRepository imageRepository,
    IGroupRepository groupRepository,
    ISecurityService securityService
) : IImageService
{
    AzureBlobStorageSettings StorageSettings => azureBlobStorageSettings.Value;
    public async Task<Image> CreateImageAsync(
        IFormFile image,
        bool isPublic,
        int? groupId) => await CreateImageDBOAsync(image, isPublic, groupId);

    public async Task<ImageDBO> CreateImageDBOAsync(
    IFormFile image,
    bool isPublic,
    int? groupId)
    {
            var user = await securityService.GetUserAsync();
        var imageExists = await GetImageAsync(image.FileName, null, user.ShortId);
        if (imageExists is not null)
            throw new InvalidOperationException("An image with the same filename already exists for this user.");
        
        if (groupId is not null)
            await groupRepository.EnsureCanEditOwn(user.ShortId, groupId);
        
        string containerName = isPublic ? StorageSettings.PublicContainerName : StorageSettings.PrivateContainerName;
        var accessType = isPublic ? PublicAccessType.Blob : PublicAccessType.None;

        // Get reference to the container
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(accessType);

        // Get reference to the blob
        var blobGuid = Guid.NewGuid().ToString();
        var blobClient = containerClient.GetBlobClient(blobGuid);

        using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        // Upload the image stream
        await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders
        {
            ContentType = image.ContentType
        });

        var imageDBO = new ImageDBO
        {
            UserShortId = user.ShortId,
            GroupId = groupId,
            IsPublic = isPublic,
            FileName = image.FileName,
            BlobGuid = blobGuid,
            ContentType = image.ContentType,
        };
        return await imageRepository.CreateImageAsync(imageDBO);
    }
    public async Task<Uri> CreateImageAndGetURIAsync(
        IFormFile image,
        bool isPublic,
        int? groupId)
    {
        var imageDBO = await CreateImageDBOAsync(image, isPublic, groupId);
        return await GetImageURI(imageDBO);
    }
    async Task<ImageDBO?> GetImageAsync(string fileName, int? imageId, int? userShortId = null)
    {
        userShortId ??= (await securityService.GetUserAsync()).ShortId;
        return await imageRepository.GetImageAsync(fileName, imageId, userShortId.Value);
    }

    public async Task<Uri> GetImageURI(string fileName, int? fileId, int? userShortId = null)
    {
        var imageDBO = await GetImageAsync(fileName, fileId, userShortId) ??
            throw new NotFoundException("The requested image does not exist.");
        
        return await GetImageURI(imageDBO);
    }
    public async Task<Uri> GetImageURI(ImageDBO imageDBO)
    {
        if (imageDBO.IsPublic)
            return new Uri($"{StorageSettings.BlobServiceEndpoint}/{StorageSettings.PublicContainerName}/{imageDBO.BlobGuid}");

        var containerClient = blobServiceClient.GetBlobContainerClient(StorageSettings.PrivateContainerName);
        var blobClient = containerClient.GetBlobClient(imageDBO.BlobGuid);

        if (!await blobClient.ExistsAsync())
            throw new NotFoundException("The requested image does not exist.");
        
        return blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(5));
    }
}
