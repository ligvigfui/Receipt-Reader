namespace RR.Service.Services;

public class SecurityService : ISecurityService
{

    private readonly JWTSettings JWTSettings;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IUserRepository userRepository;
    private readonly SigningCredentials signingCredentials;

    public SecurityService(
        IOptions<JWTSettings> jWTConfiguration,
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository
    )
    {
        JWTSettings = jWTConfiguration.Value;
        this.httpContextAccessor = httpContextAccessor;
        this.userRepository = userRepository;

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTConfiguration.Value.Key));
        signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
    }

    public async Task<string> RegisterAsync(Login login)
    {
        var newUser = await userRepository.RegisterAsync(login);

        return GetJwtToken(newUser);
    }

    public async Task<string> LoginAsync(Login user)
    {
        var userDBO = await userRepository.LoginAsync(user);

        return GetJwtToken(userDBO);
    }

    string GetJwtToken(UserDBO user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            user.UserRoles.Select(ur => new Claim("role", ur.Role.Name!)).First()
        ];

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: JWTSettings.Issuer,
            audience: JWTSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(JWTSettings.SlidingExpirationInDays),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
    public async Task<string> RefreshTokenAsync()
    {
        var userDBO = await GetUserAsync();

        var newToken = GetJwtToken(userDBO);

        return newToken;
    }

    public async Task<UserDBO> GetUserAsync()
    {
        var userEmail = httpContextAccessor?.HttpContext?.GetUserEmail();
        return await userRepository.GetUserAsync(userEmail);
    }
}
