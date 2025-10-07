namespace RR.Data.Repository;

public class UserRepository(
    UserManager<UserDBO> userManager
) : IUserRepository
{
    public async Task<UserDBO> RegisterAsync(Login user)
    {
        var newUser = new UserDBO
        {
            Email = user.Email,
            UserName = user.Email
        };
        var result = await userManager.CreateAsync(newUser, user.Password);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException(string.Join(", ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(newUser, Role.User.ToString());

        return newUser;
    }

    public async Task<UserDBO> GetUserAsync(string? email) =>
        await userManager.FindByEmailAsync(email ?? "") ??
            throw new UnauthorizedAccessException("User not found");

    public async Task<UserDBO> LoginAsync(Login user)
    {
        var userDBO = await GetUserAsync(user.Email);

        var result = await userManager.CheckPasswordAsync(userDBO, user.Password);
        if (!result)
            throw new UnauthorizedAccessException("Invalid password");

        return userDBO;
    }
}
