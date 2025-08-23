namespace RR.Data.DataBaseObjects;

[Tables(nameof(VendorDBO))]
public class VendorDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public virtual List<ReceiptDBO> Receipts { get; set; } = [];
    public int? HQId { get; set; }
    public virtual VendorHQDBO? HQ { get; set; }
    public string Name { get; set; }
    public int AddressId { get; set; }
    public virtual AddressDBO Address { get; set; }
    public string TaxNumber { get; set; }

    public static implicit operator VendorDBO(Vendor vendor) => new()
    {
        HQ = vendor.VendorHQ,
        Name = vendor.Name,
        Address = vendor.Address,
        TaxNumber = vendor.TaxNumber
    };

    public static implicit operator Vendor(VendorDBO vendorDBO) => new()
    {
        VendorHQ = vendorDBO.HQ,
        Name = vendorDBO.Name,
        Address = vendorDBO.Address,
        TaxNumber = vendorDBO.TaxNumber
    };
}
