namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VendorController(
    ILogger<VendorController> logger,
    IVendorRepository vendorRepository
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery][Required] int id) =>
        Ok(await vendorRepository.GetVendorByIdAsync(id));
}
