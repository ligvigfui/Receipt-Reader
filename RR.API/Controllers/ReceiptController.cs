namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceiptController(
    IReceiptService receiptService
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody][Required] Receipt receipt)
    {
        var newReceipt = await receiptService.CreateReceiptAsync(receipt);
        return CreatedAtAction(nameof(PostAsync), newReceipt);
    }

}
