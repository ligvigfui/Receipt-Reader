namespace RR.Data.DataBaseObjects;

[Tables(nameof(GroupDBO))]
public class GroupDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public virtual List<UserDBO> Users { get; set; }
    public virtual List<ReceiptDBO> Receipts { get; set; }
    public virtual List<ImageDBO> Images { get; set; }
    internal virtual List<UserGroupDBO> UserGroups { get; set; }
}