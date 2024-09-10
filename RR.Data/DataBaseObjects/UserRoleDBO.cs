namespace RR.Data.DataBaseObjects;

[Table(nameof(UserRoleDBO))]
public class UserRoleDBO : IdentityUserRole<string>
{
    public virtual UserDBO User { get; set; }
    public virtual RoleDBO Role { get; set; }
}
