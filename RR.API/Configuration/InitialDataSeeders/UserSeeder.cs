namespace RR.API.Configuration.InitialDataSeeders;

public class UserSeeder(IServiceProvider services, ApplicationDbContext context) : ISeedable
{
    public async Task<bool> ShouldSeed() => !await context.Users.AnyAsync();
    public async Task Seed()
    {
        var roleManager = services.GetRequiredService<RoleManager<RoleDBO>>();
        await roleManager.CreateAsync(new RoleDBO { Name = Role.Admin.ToString() });
        await roleManager.CreateAsync(new RoleDBO { Name = Role.User.ToString() });

        var userManager = services.GetRequiredService<UserManager<UserDBO>>();
        var adminUserSettings = services.GetRequiredService<IOptions<AdminUserSettings>>().Value;
        var admin = new UserDBO
        {
            UserName = adminUserSettings.UserName,
            Email = adminUserSettings.Email,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };
        await userManager.CreateAsync(admin, adminUserSettings.Password);

        await userManager.AddToRoleAsync(admin, Role.Admin.ToString());
    }
}