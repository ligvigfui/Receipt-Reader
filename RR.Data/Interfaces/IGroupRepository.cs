namespace RR.Data.Interfaces;

public interface IGroupRepository
{
    Task<UserGroupDBO> GetUserGroup(string userEmail, int groupId);
}