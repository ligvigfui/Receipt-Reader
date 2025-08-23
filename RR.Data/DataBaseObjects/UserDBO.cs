namespace RR.Data.DataBaseObjects;

[Tables(nameof(UserDBO))]
public class UserDBO : IdentityUser
{
    public virtual List<ReceiptDBO> Receipts { get; set; }
    public virtual List<UserRoleDBO> UserRoles { get; set; }
}
