namespace RR.Data.Interfaces;

public interface IGroupRepository
{
    Task<UserGroupDBO?> EnsureCanEdit(int? groupId, int? userShortId);
    Task<UserGroupDBO?> EnsureCanEditOwn(int? groupId, int? userShortId);
    Task<UserGroupDBO?> EnsureCanRead(int? groupId, int? userShortId);
    Task<UserGroupDBO?> EnsureCanReadOwn(int? groupId, int? userShortId);
    Task<UserGroupDBO?> GetUserGroup(int? groupId, int? userShortId);
}