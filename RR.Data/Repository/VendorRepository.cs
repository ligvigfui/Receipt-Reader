namespace RR.Data.Repository;

public class VendorRepository(ApplicationDbContext context) : IVendorRepository
{
    public async Task<List<VendorDBO>> GetVerndorSuggestionsAsync(string query, int maxResults = 5) =>
        await context.Vendors
            .OrderBy(vendor => ApplicationDbContext.Levenshtein(vendor.Name, query))
            .Take(maxResults)
            .ToListAsync();

    public async Task<VendorDBO?> GetVendorAsync(Vendor vendor, int userShortId) =>
        await context.Vendors.WhereCanRead(userShortId).FirstOrDefaultAsync(v => v.Id == vendor.Id && v.Name == vendor.Name);

    public async Task<VendorDBO> CreateVendorAsync(VendorDBO vendorDBO)
    {
        await context.Vendors.AddAsync(vendorDBO);
        await context.SaveChangesAsync();
        return vendorDBO;
    }

    public async Task<VendorDBO> CreateOrGetVendorAsync(VendorDBO vendorDBO, int userShortId) =>
        await GetVendorAsync(vendorDBO, userShortId) ??
            await CreateVendorAsync(vendorDBO);

}
