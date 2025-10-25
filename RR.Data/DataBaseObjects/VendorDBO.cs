namespace RR.Data.DataBaseObjects;

[Tables(nameof(VendorDBO))]
public class VendorDBO : AbstractPublicOwnable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public virtual List<ReceiptDBO> Receipts { get; set; } = [];
    public int? HQId { get; set; }
    public virtual VendorHQDBO? HQ { get; set; }
    public string Name { get; set; }
    public int? AddressId { get; set; }
    public virtual AddressDBO? Address { get; set; }

    public static implicit operator Vendor?(VendorDBO? vendorDBO) => vendorDBO is null ? null : new()
    {
        Id = vendorDBO.Id,
        GroupId = vendorDBO.GroupId,
        VendorHQ = vendorDBO.HQ,
        Name = vendorDBO.Name,
        Address = vendorDBO.Address,
    };
}
