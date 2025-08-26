namespace RR.Data.DataBaseObjects;

[Tables(nameof(UserDBO))]
public class UserDBO : IdentityUser
{
    public virtual List<GroupDBO> Groups { get; set; }
    public virtual List<ReceiptDBO> Receipts { get; set; }
    public virtual List<ImageDBO> Images { get; set; }
    public virtual List<UserRoleDBO> UserRoles { get; set; }
    internal virtual List<UserGroupDBO> UserGroups { get; set; }
}
