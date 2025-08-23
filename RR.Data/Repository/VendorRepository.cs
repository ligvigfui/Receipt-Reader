namespace RR.Data.Repository;

public class VendorRepository(ApplicationDbContext context) : IVendorRepository
{
    public async Task<VendorDBO?> GetVendorByIdAsync(int vendorId) =>
        await context.Vendors.FirstOrDefaultAsync(vendor => vendor.Id == vendorId);

    public async Task<int> CreateVendorAsync(Vendor vendor)
    {
        var vendorDBO = (VendorDBO)vendor;
        await context.Vendors.AddAsync(vendorDBO);
        await context.SaveChangesAsync();
        return vendorDBO.Id;
    }
}
