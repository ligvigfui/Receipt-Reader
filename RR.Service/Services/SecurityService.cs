namespace RR.Service.Services;

public class SecurityService(
    IOptions<JWTSettings> jWTConfiguration,
    IHttpContextAccessor httpContextAccessor,
    IUserRepository userRepository,
    IGroupRepository groupRepository
    ) : ISecurityService
{
    private readonly JWTSettings JWTSettings = jWTConfiguration.Value;
    private readonly SigningCredentials signingCredentials = new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTConfiguration.Value.Key)), SecurityAlgorithms.HmacSha256);

    public async Task<UserGroupDBO> GetUserGroup(int groupId)
    {
        var userEmail = httpContextAccessor.HttpContext!.GetUserEmail();
        return await groupRepository.GetUserGroup(userEmail, groupId);
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
