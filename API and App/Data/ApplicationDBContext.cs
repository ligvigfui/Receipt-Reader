namespace RR.Data;


public class ApplicationDbContext : IdentityDbContext<UserDto, RoleDto, string, IdentityUserClaim<string>,
    UserRoleDto, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{

}
