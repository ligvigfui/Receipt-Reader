namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceiptController(
    IReceiptService receiptService
) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> PostAsync(
        [FromBody][Required] Receipt receipt)
    {
        var newReceipt = await receiptService.CreateReceiptAsync(receipt);
        return CreatedAtAction(nameof(PostAsync), newReceipt);
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
