namespace RR.Settings;

public record JWTSettings
{
    public string Audience { get; set; }
    public short ExpirationInDays { get; set; }
    public short SlidingExpirationInDays { get; set; }
    public short RefreshTokenAfterDays { get; set; }
    public string Issuer { get; set; }
    public string Key { get; set; }
}
