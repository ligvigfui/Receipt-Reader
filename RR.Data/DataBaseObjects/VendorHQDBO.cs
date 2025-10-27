namespace RR.Data.DataBaseObjects;

[Tables(nameof(VendorHQDBO))]
public class VendorHQDBO : AbstractPublicOwnable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public int? AddressId { get; set; }
    public virtual AddressDBO? Address { get; set; }
    public string? TaxNumber { get; set; }
    public virtual List<VendorDBO> Vendors { get; set; }
    public VendorHQDBO() { }
    public VendorHQDBO(VendorHQ vendorHQ, int userShortId)
    {
        UserShortId = userShortId;
        GroupId = vendorHQ.GroupId;
        if (vendorHQ.Id is not null)
            Id = vendorHQ.Id.Value;
        Name = vendorHQ.Name;
        Address = vendorHQ.Address is null ? null : new AddressDBO(vendorHQ.Address, userShortId);
        TaxNumber = vendorHQ.TaxNumber;
    }

    public static implicit operator VendorHQ?(VendorHQDBO? vendorHQDBO) => vendorHQDBO is null ? null : new()
    {
        Id = vendorHQDBO.Id,
        GroupId = vendorHQDBO.GroupId,
        Name = vendorHQDBO.Name,
        Address = vendorHQDBO.Address,
        TaxNumber = vendorHQDBO.TaxNumber
    };
}
