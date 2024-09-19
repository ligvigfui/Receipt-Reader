namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(
    //ILogger<AccountController> logger,
    ISecurityService securityService
) : ControllerBase
{
    [HttpPost]
    [Route(nameof(Register))]
    public async Task<IActionResult> Register([FromBody] Login login)
    {
        var token = await securityService.RegisterAsync(login);
        return Ok(new AuthResponse{ Token = token });
    }

    [HttpPost]
    [Route(nameof(Login))]
    public async Task<IActionResult> Login([FromBody] Login login)
    {
        var token = await securityService.LoginAsync(login);
        return Ok(new AuthResponse{ Token = token });
    }

    [HttpPost]
    [Route(nameof(RefreshToken))]
    public async Task<IActionResult> RefreshToken()
    {
        var token = await securityService.RefreshTokenAsync();
        return CreatedAtAction(nameof(RefreshToken), new { Token = token });
    }


}
