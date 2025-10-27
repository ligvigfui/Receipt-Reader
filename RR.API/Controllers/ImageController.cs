namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController(
    IImageService imageService,
    ISecurityService securityService
) : ControllerBase
{
    [HttpPost("Upload/{isPublic}")]
    public async Task<IActionResult> UploadImage(
        bool isPublic,
        int? groupId,
        IFormFile file
    )
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");
        
        var imageDBO = await imageService.CreateImageAsync(file, isPublic, groupId);
        return CreatedAtAction(nameof(UploadImage), imageDBO);
    }
}
