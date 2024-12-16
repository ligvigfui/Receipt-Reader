using Microsoft.OpenApi.Models;

namespace RR.API.Configuration;

public static class SwaggerConfiguration
{
    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {

        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.UseInlineDefinitionsForEnums();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // Define the security scheme
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
				BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
            });

            // Add the security requirement
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });

        var swaggerSettings = builder.Configuration.GetSection("Swagger").Get<SwaggerSettings>();
        if (swaggerSettings.LaunchOnStartup)
        {
            var url = "https://localhost:5001/swagger/index.html";
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }
    }
}
