namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressController(
    //ILogger<AddressController> logger,
    IAddressRepository addressRepository
) : ControllerBase
{
    [HttpGet]
    [AuthorizeRoles(Role.User)]
    public async Task<IActionResult> Get([FromQuery][Required] int id) =>
        Ok(await addressRepository.GetAddressByIdAsync(id));

    [HttpPost]
    [Route(nameof(Create))]
    [AuthorizeRoles(Role.User)]
    public async Task<IActionResult> Create([FromBody] Address address)
    {
        var addressDBO = await addressRepository.CreateAddressAsync(address);
        return CreatedAtAction(nameof(Create), addressDBO);
    }

    
}
