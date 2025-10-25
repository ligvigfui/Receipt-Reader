namespace RR.API.Configuration;

public static class DataSeeder
{
    public static async Task SeedEssentialData(this IHost app, ApplicationDbContext context)
    {
        if (AnyMeasurments(context) && AnyUsers(context))
            return;
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        await SeedUsers(services, context);
        await SeedMeasurements(context);

        await context.SaveChangesAsync();
    }

    static bool AnyUsers(ApplicationDbContext context) => context.Users.Any();
    static async Task SeedUsers(IServiceProvider services, ApplicationDbContext context)
    {
        if (!AnyUsers(context))
            return;
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

    static bool AnyMeasurments(ApplicationDbContext context) => context.Measurments.Any();
    static async Task SeedMeasurements(ApplicationDbContext context)
    {
        if (!AnyMeasurments(context))
            return;
        await context.Measurments.AddRangeAsync(new List<MeasurementDBO>
        {
            new("Gram", "Grams", "g", MeasurementCategory.Weight, 1),
            new("Liter", "Liters", "L", MeasurementCategory.Volume, 1),
            new("Meter", "Meters", "m", MeasurementCategory.Length, 1),
            new("Piece", "Pieces", "pc", MeasurementCategory.Count, 1),

            new("Kilogram", "Kilograms", "kg", MeasurementCategory.Weight, 1000),
            new("Milliliter", "Milliliters", "mL", MeasurementCategory.Volume, 0.001f),
            new("Centimeter", "Centimeters", "cm", MeasurementCategory.Length, 0.01f),
        });
    }
}
