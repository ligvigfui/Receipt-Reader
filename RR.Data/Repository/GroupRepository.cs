using Microsoft.Extensions.Caching.Memory;

namespace RR.Data.Repository;

public class GroupRepository(
    IMemoryCache cache,
    ApplicationDbContext context
) : IGroupRepository
{
    public async Task<UserGroupDBO?> GetUserGroup(int userShortId, int? groupId)
    {
        if (groupId is null)
            return null;
        if (cache.TryGetValue((userShortId, groupId), out var value))
            return (UserGroupDBO)value!;
        
        var userGroupDBO = await context.UserGroups.FirstOrDefaultAsync(ug => ug.GroupId == groupId && ug.User.ShortId == userShortId) ??
            throw new UnauthorizedAccessException("Group does not exist or you don't have access to it.");
        cache.Set((userShortId, groupId), userGroupDBO, TimeSpan.FromSeconds(5));
        return userGroupDBO;
    }

    public async Task<UserGroupDBO?> EnsureCanRead(int userShortId, int? groupId)
    {
        var userGroup = await GetUserGroup(userShortId, groupId);
        if (!userGroup?.CanRead ?? false)
            throw new UnauthorizedAccessException($"You do not have permission to access others resources in group: '{userGroup.Group.Name}'.");
        return userGroup;
    }
    public async Task<UserGroupDBO?> EnsureCanReadOwn(int userShortId, int? groupId)
    {
        var userGroup = await GetUserGroup(userShortId, groupId);
        if (!userGroup?.CanReadOwn ?? false)
            throw new UnauthorizedAccessException($"You do not have permission to access your own resources in group: '{userGroup.Group.Name}'.");
        return userGroup;
    }
    public async Task<UserGroupDBO?> EnsureCanEdit(int userShortId, int? groupId)
    {
        var userGroup = await GetUserGroup(userShortId, groupId);
        if (!userGroup?.CanEdit ?? false)
            throw new UnauthorizedAccessException($"You do not have permissions to edit others resources in group: '{userGroup.Group.Name}'.");
        return userGroup;
    }
    public async Task<UserGroupDBO?> EnsureCanEditOwn(int userShortId, int? groupId)
    {
        var userGroup = await GetUserGroup(userShortId, groupId);
        if (!userGroup?.CanEditOwn ?? false)
            throw new UnauthorizedAccessException($"You do not have permissions to add or edit your resources in group: '{userGroup.Group.Name}'.");
        return userGroup;
    }
}
