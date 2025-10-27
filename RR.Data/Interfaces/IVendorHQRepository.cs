namespace RR.Data.Interfaces;

public interface IVendorHQRepository
{
    Task<VendorHQDBO> CreateOrGetVendorHQAsync(VendorHQDBO vendorHQDBO);
    Task<VendorHQDBO> CreateVendorHQAsync(VendorHQDBO vendorHQDBO);
    Task<VendorHQDBO?> GetVendorHQAsync(VendorHQ? vendorHQ, int userShortId);
}
