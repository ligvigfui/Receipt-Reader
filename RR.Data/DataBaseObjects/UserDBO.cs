namespace RR.Data.DataBaseObjects;

[Tables(nameof(UserDBO))]
public class UserDBO : IdentityUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ShortId { get; set; }
    public bool AreItemsDefaultPublic { get; set; } = true;
    [StringLength(5)]
    public string? DefaultLanguage { get; set; }
    public virtual List<GroupDBO> Groups { get; set; }
    public virtual List<UserRoleDBO> UserRoles { get; set; }
    internal virtual List<UserGroupDBO> UserGroups { get; set; }
}
