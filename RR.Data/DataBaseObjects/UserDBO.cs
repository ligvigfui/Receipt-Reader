using System.Text.Json.Serialization;

namespace RR.Data.DataBaseObjects;

[Tables(nameof(UserDBO))]
public class UserDBO : IdentityUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ShortId { get; set; }
    public bool AreItemsDefaultPublic { get; set; } = true;
    public byte DefaultLanguageId { get; set; }
    public virtual IEnumerable<GroupDBO> Groups { get; set; }
    [JsonIgnore]
    public virtual IEnumerable<UserRoleDBO> UserRoles { get; set; }
    internal virtual IEnumerable<UserGroupDBO> UserGroups { get; set; }
}
