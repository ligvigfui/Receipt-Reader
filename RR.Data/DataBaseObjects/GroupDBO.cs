namespace RR.Data.DataBaseObjects;

[Tables(nameof(GroupDBO))]
public class GroupDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool AreItemsDefaultPublic { get; set; }
    public virtual List<UserDBO> Users { get; set; }
    public virtual List<UserGroupDBO> UserGroups { get; set; }
}