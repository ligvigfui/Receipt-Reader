namespace RR.Data.Interfaces;

public interface IVendorRepository
{
    public Task<VendorDBO?> GetVendorByIdAsync(int vendorId);
}
