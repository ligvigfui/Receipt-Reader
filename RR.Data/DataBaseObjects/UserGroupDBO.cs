namespace RR.Data.DataBaseObjects;

[Tables(nameof(UserGroupDBO))]
[PrimaryKey(nameof(UserId), nameof(GroupId))]
public class UserGroupDBO
{
    public string UserId { get; set; }
    public virtual UserDBO User { get; set; }
    public int GroupId { get; set; }
    public virtual GroupDBO Group { get; set; }
    public bool CanRead { get; set; } = true;
    public bool CanReadOwn { get; set; } = true;
    public bool CanEdit { get; set; } = false;
    public bool CanEditOwn { get; set; } = true;
    public bool CanAddMembers { get; set; } = true;
    public bool CanRemoveMembers { get; set; } = false;
    public bool CanEditGroupPermissions { get; set; } = false;

    public void AsAdmin()
    {
        CanRead = true;
        CanReadOwn = true;
        CanEdit = true;
        CanEditOwn = true;
        CanAddMembers = true;
        CanRemoveMembers = true;
        CanEditGroupPermissions = true;
    }
}