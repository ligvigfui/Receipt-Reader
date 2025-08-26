namespace RR.Data.Interfaces;

public interface IVendorRepository
{
    Task<int> CreateVendorAsync(VendorDBO vendor);
    public Task<VendorDBO?> GetVendorByIdAsync(int vendorId);
}
