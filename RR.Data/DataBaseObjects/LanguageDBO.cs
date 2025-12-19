namespace RR.Data.DataBaseObjects;

[Tables(nameof(LanguageDBO))]
public class LanguageDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ushort Id { get; set; }
    [StringLength(5)]
    public string LanguageCode { get; set; }
    public string LanguageName { get; set; }
    public string CultureName { get; set; }
}
