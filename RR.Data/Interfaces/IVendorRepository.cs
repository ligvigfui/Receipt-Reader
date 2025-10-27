namespace RR.Data.Interfaces;

public interface IVendorRepository
{
    Task<VendorDBO> GetOrCreateVendorAsync(VendorDBO vendorDBO);
    Task<VendorDBO> CreateVendorAsync(VendorDBO vendorDBO);
    Task<VendorDBO?> GetVendorAsync(Vendor? vendor, int userShortId);
}
