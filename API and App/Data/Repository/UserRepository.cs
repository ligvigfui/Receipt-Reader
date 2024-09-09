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
            throw new Exception("Failed to create user");
        
        await userManager.AddToRoleAsync(newUser, Role.User.ToString());

        return newUser;
    }

    public async Task<UserDBO> GetUserAsync(string? email) =>
        await userManager.FindByEmailAsync(email ?? "") ??
            throw new Exception("User not found");

    public async Task<UserDBO> LoginAsync(Login user)
    {
        var userDBO = await userManager.FindByEmailAsync(user.Email) ??
            throw new Exception("User not found");

        var result = await userManager.CheckPasswordAsync(userDBO, user.Password);
        if (!result)
            throw new Exception("Invalid password");

        return userDBO;
    }
}
