namespace RR.Service.Interfaces;

public interface ISecurityService
{
    Task<UserDBO> GetUserAsync();
    Task<UserGroupDBO> GetUserGroup(int groupId);
    Task<string> LoginAsync(Login user);
    Task<string> RefreshTokenAsync();
    Task<string> RegisterAsync(Login login);
}
