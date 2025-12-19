namespace RR.Data.Interfaces;

public interface IGroupRepository
{
    Task<UserGroupDBO?> EnsureCanEdit(int userShortId, int? groupId);
    Task<UserGroupDBO?> EnsureCanEditOwn(int userShortId, int? groupId);
    Task<UserGroupDBO?> EnsureCanRead(int userShortId, int? groupId);
    Task<UserGroupDBO?> EnsureCanReadOwn(int userShortId, int? groupId);
    Task<UserGroupDBO?> GetUserGroup(int userShortId, int? groupId);
}