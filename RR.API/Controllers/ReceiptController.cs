namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceiptController(
    IReceiptService receiptService
) : ControllerBase
{
    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create(
        [FromBody][Required] Receipt receipt)
    {
        var newReceipt = await receiptService.CreateReceiptAsync(receipt);
        return CreatedAtAction(nameof(Create), newReceipt);
    }

    [HttpPost(nameof(CreateFromSavedImage))]
    [Produces<ReceiptValidated>]
    public async Task<IActionResult> CreateFromSavedImage(
        bool isPublic,
        string imageName,
        int? imageId,
        int? groupId,
        string? languageCode)
    {
        var newReceipt = await receiptService.CreateReceiptFromImageAsync(imageName, imageId, groupId);
        return CreatedAtAction(nameof(CreateFromSavedImage), newReceipt);
    }

    [HttpPost(nameof(CreateFromImage))]
    [Produces<ReceiptValidated>]
    public async Task<IActionResult> CreateFromImage(
        [FromQuery] bool isImagePublic,
        [FromQuery] bool isReceiptPublic,
        [FromQuery] int? groupId,
        [FromQuery] string? languageCode,
        IFormFile image)
    {
        var newReceipt = await receiptService.CreateReceiptFromImageAsync(isPublic, groupId, image);
        return CreatedAtAction(nameof(CreateFromImage), newReceipt);
    }
    //[HttpGet("Get")]
    //public async Task<IActionResult> GetAsync(
    //    int? groupId = null,
    //    DateTime? startDate = null,
    //    DateTime? endDate = null)
    //{
    //    var receipts = await receiptService.GetReceiptsAsync(groupId, startDate, endDate);
    //    return Ok(receipts);
    //}

    //[HttpPatch("Update")]
    //public async Task<IActionResult> Update([FromBody][Required] Receipt receipt)
    //{

    //}

}
