namespace RR.Data.DataBaseObjects;

public abstract class AbstractPublicOwnable
{
    public int? UserShortId { get; set; }
    public virtual UserDBO? User { get; set; }
    public int? GroupId { get; set; }
    public virtual GroupDBO? Group { get; set; }
    public bool IsPublic { get; set; }
    //public bool CanRead(int userShortId)
    //{
    //    if (IsPublic)
    //        return true;
    //    if (GroupId is null)
    //        return UserShortId == userShortId;
    //    var userGroup = Group?.UserGroups.Where(ug => ug.UserShortId == userShortId).FirstOrDefault();
    //    if (userGroup is null)
    //        return false;
    //    if (userShortId == UserShortId)
    //        return userGroup.CanReadOwn;
    //    else
    //        return userGroup.CanRead;
    //}

    //public bool CanEdit(int userShortId)
    //{
    //    if (GroupId is null)
    //        return UserShortId == userShortId;
    //    var userGroup = Group?.UserGroups.Where(ug => ug.User.ShortId == userShortId).FirstOrDefault();
    //    if (userGroup is null)
    //        return false;
    //    if (userShortId == UserShortId)
    //        return userGroup.CanEditOwn;
    //    else
    //        return userGroup.CanEdit;
    //}
}
public static class AbstractPublicOwnableExtensions
{
    public static IQueryable<T> WhereCanRead<T>(this IQueryable<T> dbSet, int userShortId)
        where T : AbstractPublicOwnable => dbSet
            .Where(x => x.IsPublic
                || (x.GroupId == null && x.UserShortId == userShortId)
                || (x.Group.UserGroups.Any(ug => ug.UserShortId == userShortId &&
                    ((x.UserShortId == userShortId && ug.CanReadOwn) || (x.UserShortId != userShortId && ug.CanRead)))));
    public static IQueryable<T> WhereCanEdit<T>(this IQueryable<T> dbSet, int userShortId)
        where T : AbstractPublicOwnable => dbSet
            .Where(x => (x.GroupId == null && x.UserShortId == userShortId)
                || (x.Group.UserGroups.Any(ug => ug.UserShortId == userShortId &&
                    ((x.UserShortId == userShortId && ug.CanEditOwn) || (x.UserShortId != userShortId && ug.CanEdit)))));

}