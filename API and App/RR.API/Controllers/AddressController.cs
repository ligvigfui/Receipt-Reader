namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressController(
    //ILogger<AddressController> logger,
    IAddressRepository addressRepositry
) : ControllerBase
{
    [HttpGet]
    [AuthorizeRoles(Role.User)]
    public async Task<IActionResult> Get([FromQuery][Required] int id) =>
        Ok(await addressRepositry.GetAddressByIdAsync(id));

    [HttpPost]
    [Route(nameof(Create))]
    [AuthorizeRoles(Role.User)]
    public async Task<IActionResult> Create([FromBody] Address address)
    {
        var addressDBO = await addressRepositry.CreateAddressAsync(address);
        return CreatedAtAction(nameof(Create), addressDBO);
    }

    
}
