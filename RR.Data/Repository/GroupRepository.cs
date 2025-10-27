using Microsoft.Extensions.Caching.Memory;

namespace RR.Data.Repository;

public class GroupRepository(
    IMemoryCache cache,
    ApplicationDbContext context
) : IGroupRepository
{
    public async Task<UserGroupDBO?> GetUserGroup(int? groupId, int? userShortId)
    {
        if (userShortId is null)
            throw new UnauthorizedAccessException("User must be authenticated.");
        if (groupId is null)
            return null;
        if (cache.TryGetValue((groupId, userShortId), out var value))
            return (UserGroupDBO)value!;
        
        var userGroupDBO = await context.UserGroups.FirstOrDefaultAsync(ug => ug.GroupId == groupId && ug.User.ShortId == userShortId) ??
            throw new UnauthorizedAccessException("Group does not exist or you don't have access to it.");
        cache.Set((groupId, userShortId), userGroupDBO, TimeSpan.FromSeconds(5));
        return userGroupDBO;
    }

    public async Task<UserGroupDBO?> EnsureCanRead(int? groupId, int? userShortId)
    {
        var userGroup = await GetUserGroup(groupId, userShortId);
        if (!userGroup?.CanRead ?? true)
            throw new UnauthorizedAccessException($"You do not have permission to access others resources in group: '{userGroup.Group.Name}'.");
        return userGroup;
    }
    public async Task<UserGroupDBO?> EnsureCanReadOwn(int? groupId, int? userShortId)
    {
        var userGroup = await GetUserGroup(groupId, userShortId);
        if (!userGroup?.CanReadOwn ?? true)
            throw new UnauthorizedAccessException($"You do not have permission to access your own resources in group: '{userGroup.Group.Name}'.");
        return userGroup;
    }
    public async Task<UserGroupDBO?> EnsureCanEdit(int? groupId, int? userShortId)
    {
        var userGroup = await GetUserGroup(groupId, userShortId);
        if (!userGroup?.CanEdit ?? true)
            throw new UnauthorizedAccessException($"You do not have permissions to edit others resources in group: '{userGroup.Group.Name}'.");
        return userGroup;
    }
    public async Task<UserGroupDBO?> EnsureCanEditOwn(int? groupId, int? userShortId)
    {
        var userGroup = await GetUserGroup(groupId, userShortId);
        if (!userGroup?.CanEditOwn ?? true)
            throw new UnauthorizedAccessException($"You do not have permissions to add or edit your resources in group: '{userGroup.Group.Name}'.");
        return userGroup;
    }
}
