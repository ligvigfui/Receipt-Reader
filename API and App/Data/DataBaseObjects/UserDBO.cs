namespace RR.Data.DataBaseObjects;

[Table(nameof(UserDBO))]
public class UserDBO : IdentityUser
{
    public List<ReceiptDBO> Receipts { get; set; }
    public List<UserRoleDBO> UserRoles { get; set; }
}
