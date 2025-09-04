namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceiptController(
    IReceiptService receiptService
) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> PostAsync(
        [FromBody][Required] Receipt receipt,
        string? groupName = null)
    {
        var newReceipt = await receiptService.CreateReceiptAsync(receipt, groupName);
        return CreatedAtAction(nameof(PostAsync), newReceipt);
    }

}
