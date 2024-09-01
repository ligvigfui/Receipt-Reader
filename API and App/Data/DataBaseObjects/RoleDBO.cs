namespace RR.Data.DataBaseObjects;

[Table(nameof(RoleDBO))]
public class RoleDBO : IdentityRole
{
    public List<UserRoleDBO> UserRoles { get; set; }
}