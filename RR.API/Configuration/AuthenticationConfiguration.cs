namespace RR.API.Configuration;

public static class AuthenticationConfiguration
{
    public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        var jwtConfiguration = builder.Configuration.GetSection("JWT").Get<JWTConfiguration>()!;


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
				ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key))
            };
        });

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("JWT", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });

        HttpContextExtension.KEY = jwtConfiguration.Key;
        
        return builder;
    }
}
