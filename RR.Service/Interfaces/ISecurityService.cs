namespace RR.Service.Interfaces;

public interface ISecurityService
{
    Task<UserGroupDBO?> EnsureCanEdit(IOwnable ownable);
    Task<UserGroupDBO?> EnsureCanEditOwn(IOwnable ownable);
    Task<UserGroupDBO?> EnsureCanRead(IOwnable ownable);
    Task<UserGroupDBO?> EnsureCanReadOwn(IOwnable ownable);
    Task<UserDBO> GetUserAsync();
    Task<UserGroupDBO?> GetUserGroup(int? groupId);
    Task<string> LoginAsync(Login user);
    Task<string> RefreshTokenAsync();
    Task<string> RegisterAsync(Login login);
}
