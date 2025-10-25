namespace RR.Data.Repository;

public class VendorHQRepository(
    ApplicationDbContext context
) : IVendorHQRepository
{
    public async Task<VendorHQDBO?> GetVendorHQAsync(VendorHQ vendorHQ, int userShortId) =>
        await context.VendorHQs.WhereCanRead(userShortId).FirstOrDefaultAsync(vh => vh.Id == vendorHQ.Id && vh.Name == vendorHQ.Name);
    public async Task<VendorHQDBO> CreateVendorHQAsync(VendorHQDBO vendorHQDBO)
    {
        await context.VendorHQs.AddAsync(vendorHQDBO);
        await context.SaveChangesAsync();
        return vendorHQDBO;
    }
    public async Task<VendorHQDBO> CreateOrGetVendorHQAsync(VendorHQDBO vendorHQDBO, int userShortId) =>
        await GetVendorHQAsync(vendorHQDBO, userShortId) ??
            await CreateVendorHQAsync(vendorHQDBO);
}
