using Microsoft.AspNetCore.Identity.Data;

namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(
    //ILogger<AccountController> logger,
    IUserRepository userRepo
) : ControllerBase
{
    //[HttpPost]
    //[Route(nameof(Register))]
    //public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    //{
    //    var user = await userRepo.RegisterAsync(request);
    //    return CreatedAtAction(nameof(Register), user);
    //}
}
