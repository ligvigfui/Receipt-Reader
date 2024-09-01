namespace RR.Data.DataBaseObjects;

[Table(nameof(VendorHQDBO))]
public class VendorHQDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public List<VendorDBO> VendorDBOs { get; set; } = [];
    public string Name { get; set; }
    public int AddressId { get; set; }
    public AddressDBO Address { get; set; }

    public static implicit operator VendorHQDBO(VendorHQ vendorHQ) => new()
    {
        Name = vendorHQ.Name,
        Address = vendorHQ.Address,
    };

    public static implicit operator VendorHQ(VendorHQDBO vendorHQDBO) => new()
    {
        Name = vendorHQDBO.Name,
        Address = vendorHQDBO.Address,
    };
}
