using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        listenOptions.UseHttps();
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.ConfigureAppSettings()
    .ConfigureDatabase()
    .ConfigureAuthentication()
    .ConfigureSwagger();
builder.Services.ConfigureDependencyInjection();
GlobalErrorResponse.IsDevelopment = builder.Environment.IsDevelopment();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdministratorRole", policy => policy.RequireRole(Role.Admin.ToString()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();

//Auto migration
using var serviceScope = (app as IApplicationBuilder).ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
await context!.Database.MigrateAsync();

if (!context.Users.Any())
    await app.SeedEssentialData(context);

var cultureInfo = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

app.Run();
