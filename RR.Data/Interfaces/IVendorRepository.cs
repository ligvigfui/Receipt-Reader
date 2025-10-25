namespace RR.Data.Interfaces;

public interface IVendorRepository
{
    Task<int> CreateVendorAsync(VendorDBO vendor);
    public Task<VendorDBO?> GetVendorAsync(Vendor vendor, int userShortId);
}
