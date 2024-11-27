namespace RR.API;

[DoNotAutoRegister]
public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params Role[] roles)
    {
        Roles = $"{Role.Admin}, {string.Join(",", roles.Select(x => x.ToString()))}";
    }
}
