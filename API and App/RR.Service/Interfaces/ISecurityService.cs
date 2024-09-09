namespace RR.Service.Interfaces;

public interface ISecurityService
{
    Task<string> LoginAsync(Login user);
    Task<string> RefreshTokenAsync();
    Task<string> RegisterAsync(Login login);
}
