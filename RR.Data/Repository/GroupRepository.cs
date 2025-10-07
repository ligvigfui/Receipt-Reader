namespace RR.Data.Repository;

public class GroupRepository(ApplicationDbContext context) : IGroupRepository
{
    public async Task<UserGroupDBO> GetUserGroup(string userEmail, int groupId) =>
        await context.UserGroups.FirstOrDefaultAsync(ug => ug.GroupId == groupId && ug.User.Email == userEmail) ??
            throw new UnauthorizedAccessException("Group does not exist or you don't have access to it");
}
