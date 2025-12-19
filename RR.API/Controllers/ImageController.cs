namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController(
    IImageService imageService
) : ControllerBase
{
    [HttpPost("Upload")]
    [ProducesResponse<Image>()]
    public async Task<IActionResult> UploadImage(
        [FromQuery] bool isPublic,
        [FromQuery] int? groupId,
        IFormFile file
    )
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var image = await imageService.CreateImageAsync(file, isPublic, groupId);
        return CreatedAtAction(nameof(UploadImage), image);
    }

    [HttpGet("Url")]
    [ProducesResponse<string>()]
    public async Task<IActionResult> GetImageUrl(string imageName)
    {
        var imageUrl = await imageService.GetImageURI(imageName, null);
        if (imageUrl == null)
            return NotFound();

        return Ok(imageUrl);
    }
}
