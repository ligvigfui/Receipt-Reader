namespace RR.Data.Repository;

public class VendorRepository(ApplicationDbContext context) : IVendorRepository
{
    public async Task<List<VendorDBO>> GetVerndorSuggestionsAsync(string query, int maxResults = 5) =>
        await context.Vendors
            .OrderBy(vendor => ApplicationDbContext.Levenshtein(vendor.Name, query))
            .Take(maxResults)
            .ToListAsync();

    public async Task<VendorDBO?> GetVendorByIdAsync(int vendorId) =>
        await context.Vendors.FirstOrDefaultAsync(vendor => vendor.Id == vendorId);
    public async Task<VendorDBO?> GetVendorAsync(string? name, Address? address) =>
        await context.Vendors.FirstOrDefaultAsync(vendor =>  vendor.Name == name);

    public async Task<int> CreateVendorAsync(VendorDBO vendor)
    {
        await context.Vendors.AddAsync(vendor);
        await context.SaveChangesAsync();
        return vendor.Id;
    }
}
