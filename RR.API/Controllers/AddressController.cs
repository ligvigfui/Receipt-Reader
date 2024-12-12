namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
[AuthorizeRoles(Role.User)]
public class AddressController(
    //ILogger<AddressController> logger,
    IAddressRepository addressRepository
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery][Required] int id) =>
        Ok(await addressRepository.GetAddressByIdAsync(id));

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<IActionResult> Create([FromBody] Address address) =>
        CreatedAtAction(nameof(Create), await addressRepository.CreateAddressAsync(address));
}
