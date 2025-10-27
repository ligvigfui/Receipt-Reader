namespace RR.Data.DataBaseObjects;

[Tables(nameof(VendorDBO))]
public class VendorDBO : AbstractPublicOwnable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int? HQId { get; set; }
    public virtual VendorHQDBO? HQ { get; set; }
    public string Name { get; set; }
    public int? AddressId { get; set; }
    public virtual AddressDBO? Address { get; set; }
    public virtual List<ReceiptDBO> Receipts { get; set; }
    public VendorDBO() { }
    public VendorDBO(Vendor vendor, int userShortId)
    {
        UserShortId = userShortId;
        GroupId = vendor.GroupId;
        if (vendor.Id is not null)
            Id = vendor.Id.Value;
        HQ = vendor.VendorHQ is null ? null : new VendorHQDBO(vendor.VendorHQ, userShortId);
        Name = vendor.Name;
        Address = vendor.Address is null ? null : new AddressDBO(vendor.Address, userShortId);
    }

    public static implicit operator Vendor?(VendorDBO? vendorDBO) => vendorDBO is null ? null : new()
    {
        Id = vendorDBO.Id,
        GroupId = vendorDBO.GroupId,
        VendorHQ = vendorDBO.HQ,
        Name = vendorDBO.Name,
        Address = vendorDBO.Address,
    };
}
