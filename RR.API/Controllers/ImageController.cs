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
        if (groupId is not null)
        {
            var userGroup = await securityService.GetUserGroup(groupId.Value);
            if (!userGroup.CanEditOwn)
                throw new UnauthorizedAccessException("You don't have access to add receipts to this group");
        }
        
        var imageDBO = await imageService.CreateImageAsync(file, isPublic, groupId);
        return CreatedAtAction(nameof(UploadImage), imageDBO);
    }
}
