namespace RR.Data.DataBaseObjects;

[Table(nameof(UserRoleDBO))]
public class UserRoleDBO : IdentityUserRole<string>
{
    public UserDBO User { get; set; }
    public RoleDBO Role { get; set; }
}
