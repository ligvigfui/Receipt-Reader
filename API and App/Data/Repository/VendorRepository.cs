namespace RR.Data.Repository;

public class VendorRepository(ApplicationDbContext context) : IVendorRepository
{
    public async Task<VendorDBO?> GetVendorByIdAsync(int vendorId) =>
        await context.Vendors.FirstOrDefaultAsync(vendor => vendor.Id == vendorId);
}
