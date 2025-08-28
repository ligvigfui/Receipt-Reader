namespace RR.Data.DataBaseObjects;

[Tables(nameof(ImageDBO))]
public class ImageDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string UserId { get; set; }
    public virtual UserDBO User { get; set; }
    public int? GroupId { get; set; }
    public virtual GroupDBO? Group { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public byte[] Data { get; set; }
}
