namespace RR.Data.DataBaseObjects;

[Tables(nameof(UserGroupDBO))]
[PrimaryKey(nameof(UserId), nameof(GroupId))]
public class UserGroupDBO
{
    public string UserId { get; set; }
    public virtual UserDBO User { get; set; }
    public int GroupId { get; set; }
    public virtual GroupDBO Group { get; set; }
    public UserGroupRole UserGroupRole { get; set; }
}

public enum UserGroupRole
{
    Member,
    Admin,
    Owner
}