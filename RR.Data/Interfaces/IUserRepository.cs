namespace RR.Data.Interfaces;

public interface IUserRepository
{
    Task<UserDBO> LoginAsync(Login user);
    Task<UserDBO> GetUserAsync(string? email);
    Task<UserDBO> RegisterAsync(Login user);
}
