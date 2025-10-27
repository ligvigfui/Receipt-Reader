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
    public async Task<ImageDBO> CreateImageAsync(
        IFormFile image,
        bool isPublic,
        int? groupId)
    {
        var user = await securityService.GetUserAsync();
        if (groupId is not null)
        {
            await groupRepository.EnsureCanEditOwn(groupId, user.ShortId);
        }
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

    public async Task<Uri> GetImageURI(string fileName)
    {
        var userShortId = (await securityService.GetUserAsync()).ShortId;

        var image = await imageRepository.GetImageBlobUrlAsync(fileName, userShortId) ??
            throw new NotFoundException("The requested image does not exist.");
        if (image.IsPublic)
            return new Uri($"{StorageSettings.BlobServiceEndpoint}/{StorageSettings.PublicContainerName}/{image.BlobGuid}");

        // Get reference to the private container
        var containerClient = blobServiceClient.GetBlobContainerClient(StorageSettings.PrivateContainerName);
        var blobClient = containerClient.GetBlobClient(image.BlobGuid);

        // Check if the blob exists
        if (!await blobClient.ExistsAsync())
            throw new NotFoundException("The requested image does not exist.");
        
        return blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(5));
    }
}
