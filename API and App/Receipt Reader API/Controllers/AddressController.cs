namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressController(
    //ILogger<AddressController> logger,
    IAddressRepository addressRepositry
) : ControllerBase
{
    [HttpGet]
    [AuthorizeRolesAttribute(Role.Admin)]
    public async Task<IActionResult> Get([FromQuery][Required] int id) =>
        Ok(await addressRepositry.GetAddressByIdAsync(id));

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<IActionResult> Create([FromBody] Address address)
    {
        var addressDBO = await addressRepositry.CreateAddressAsync(address);
        return CreatedAtAction(nameof(Create), addressDBO);
    }

    
}
