namespace RR.Service.Extensions;

public static class HttpContextExtension
{
    public static string KEY { private get; set; }

    public static string GetUserEmail(this HttpContext context)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last() ?? "";

        if (string.IsNullOrEmpty(token))
            throw new UnauthorizedAccessException();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(KEY);
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;
    }
}
