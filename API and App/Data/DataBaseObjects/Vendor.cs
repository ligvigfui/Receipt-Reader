namespace RR.Data.DataBaseObjects;

[Table(nameof(Vendor))]
public class Vendor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string HQName { get; set; }
    public string HQAddress { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string TaxNumber { get; set; }
}
