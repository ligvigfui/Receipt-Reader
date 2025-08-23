
namespace RR.Data.Interfaces;

public interface IVendorRepository
{
    Task<int> CreateVendorAsync(Vendor vendor);
    public Task<VendorDBO?> GetVendorByIdAsync(int vendorId);
}
