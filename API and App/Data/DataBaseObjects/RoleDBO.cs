﻿namespace RR.Data.DataBaseObjects;

[Table(nameof(RoleDBO))]
public class RoleDBO : IdentityRole
{
    public virtual List<UserRoleDBO> UserRoles { get; set; }
}