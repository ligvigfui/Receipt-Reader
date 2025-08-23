namespace RR.Data.DataBaseObjects;

[Tables(nameof(RoleDBO))]
public class RoleDBO : IdentityRole
{
    public virtual List<UserRoleDBO> UserRoles { get; set; }
}