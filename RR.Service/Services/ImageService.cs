using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RR.Service.Services;

public class ImageService(
    BlobServiceClient blobServiceClient,
    IOptions<AzureBlobStorageSettings> azureBlobStorageSettings,
    IImageRepository imageRepository,
    ISecurityService securityService
) : IImageService
{
    public async Task<ImageDBO> CreateImageAsync(
        IFormFile image,
        bool isPublic,
        int? groupId)
    {
        string containerName = isPublic ? azureBlobStorageSettings.Value.PublicContainerName : azureBlobStorageSettings.Value.PrivateContainerName;
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

        // Create ImageBlobDBO to store blob URL and metadata
        var imageDBO = new ImageDBO
        {
            FileName = image.FileName,
            BlobGuid = blobGuid,
            ContentType = image.ContentType,
            IsPublic = isPublic,
            GroupId = groupId
            // Add other metadata as needed
        };
        return await imageRepository.CreateImageAsync(imageDBO);
    }

    public async Task<Uri> GetImageURI(string fileName, int? groupId)
    {
        string userId = (await securityService.GetUserAsync()).Id;
        if (groupId is not null)
        {
            var userGroup = await securityService.GetUserGroup(groupId.Value);
            if (userGroup.UserId != userId)
            {
                if (!userGroup.CanRead)
                    throw new UnauthorizedAccessException("Please request read access from a group administrator");
                userId = userGroup.UserId;
            }
        }

        var blobGuid = await imageRepository.GetImageBlobUrlAsync(fileName, userId, groupId) ??
            throw new NotFoundException("The requested image does not exist. Did you search in the correct group?");

        // Get reference to the private container
        var containerClient = blobServiceClient.GetBlobContainerClient(azureBlobStorageSettings.Value.PrivateContainerName);
        var blobClient = containerClient.GetBlobClient(blobGuid);

        // Check if the blob exists
        if (!await blobClient.ExistsAsync())
            throw new NotFoundException("The requested image does not exist.");
        
        // Download the blob content as a stream
        return blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(5));
    }
}
