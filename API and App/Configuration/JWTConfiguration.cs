namespace RR.Configuration;

public record JWTConfiguration
{
    public string Audience { get; set; }
    public double DurationInMinutes { get; set; }
    public string Issuer { get; set; }
    public string Key { get; set; }
}
