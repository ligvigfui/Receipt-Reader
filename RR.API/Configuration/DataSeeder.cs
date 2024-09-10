namespace RR.API.Configuration;

public static class DataSeeder
{
    public static async Task SeedEssentialData(this WebApplication app, ApplicationDbContext context)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var roleManager = services.GetRequiredService<RoleManager<RoleDBO>>();
        await roleManager.CreateAsync(new RoleDBO { Name = Role.Admin.ToString() });
        await roleManager.CreateAsync(new RoleDBO { Name = Role.User.ToString() });

        var userManager = services.GetRequiredService<UserManager<UserDBO>>();
        var admin = new UserDBO {
            UserName = "admin",
            Email = "ligvigfui@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };
        await userManager.CreateAsync(admin, "Password123!");

        await userManager.AddToRoleAsync(admin, Role.Admin.ToString());

        await context.SaveChangesAsync();
    }
}
