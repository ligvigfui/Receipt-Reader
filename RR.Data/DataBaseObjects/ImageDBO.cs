namespace RR.Data.DataBaseObjects;

[Tables(nameof(ImageDBO))]
public class ImageDBO : AbstractPublicOwnable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string BlobGuid { get; set; }
}
