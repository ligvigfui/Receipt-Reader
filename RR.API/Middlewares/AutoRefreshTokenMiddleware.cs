namespace RR.API.Middlewares;

public class AutoRefreshTokenMiddleware(
    ISecurityService securityService,
    IOptions<JWTSettings> jWTConfiguration
) : IMiddleware
{
    readonly JWTSettings JWTSettings = jWTConfiguration.Value;
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        if (!string.IsNullOrEmpty(token))
        {
            var securityToken = new JwtSecurityTokenHandler().ReadToken(token);
            var tokenIsAtleastRefreshTokenDaysOld = securityToken.ValidFrom.AddDays(JWTSettings.RefreshTokenAfterDays) < DateTime.UtcNow;
            var tokenIsNotExpiredSliding = securityToken.ValidTo > DateTime.UtcNow;
            var tokenIsNotExpired = securityToken.ValidFrom.AddDays(JWTSettings.ExpirationInDays) > DateTime.UtcNow;
            if (tokenIsAtleastRefreshTokenDaysOld && tokenIsNotExpiredSliding && tokenIsNotExpired)
            {
                var newToken = await securityService.RefreshTokenAsync();
                context.Response.Headers.Authorization = newToken;
            }
        }
        await next(context);
    }
}
