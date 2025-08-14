using Azure.Storage.Blobs;

namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UploadController : ControllerBase
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public UploadController(IConfiguration config)
    {
        _connectionString = config["AzureBlobStorage:ConnectionString"];
        _containerName = config["AzureBlobStorage:ContainerName"];
    }

    [HttpPost("image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        var blobClient = new BlobContainerClient(_connectionString, _containerName);
        await blobClient.CreateIfNotExistsAsync();
        var blob = blobClient.GetBlobClient(file.FileName);

        using var stream = file.OpenReadStream();
        await blob.UploadAsync(stream, overwrite: true);

        return Ok(new { url = blob.Uri.ToString() });
    }
}