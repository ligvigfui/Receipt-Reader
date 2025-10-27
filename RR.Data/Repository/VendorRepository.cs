namespace RR.Data.Repository;

public class VendorRepository(
    IVendorHQRepository vendorHQRepository,
    IAddressRepository addressRepository,
    IGroupRepository groupRepository,
    ApplicationDbContext context
) : IVendorRepository
{
    public async Task<List<VendorDBO>> GetVerndorSuggestionsAsync(string query, int maxResults = 5) =>
        await context.Vendors
            .OrderBy(vendor => ApplicationDbContext.Levenshtein(vendor.Name, query))
            .Take(maxResults)
            .ToListAsync();

    public async Task<VendorDBO?> GetVendorAsync(Vendor? vendor, int userShortId) => vendor is null ? null :
        await context.Vendors
            .WhereCanRead(userShortId)
            .Include(v => v.HQ)
                .ThenInclude(vh => vh.Address)
            .Include(v => v.Address)
            .FirstOrDefaultAsync(v => v.Id == vendor.Id && v.Name == vendor.Name);

    public async Task<VendorDBO> CreateVendorAsync(VendorDBO vendorDBO)
    {
        await groupRepository.EnsureCanEditOwn(vendorDBO.GroupId, vendorDBO.UserShortId);
        var address = await addressRepository.GetAddressAsync(vendorDBO.Address, vendorDBO.UserShortId!.Value);
        if (address is not null)
            vendorDBO.Address = address;
        else if (vendorDBO.Address is not null)
            vendorDBO.Address.UserShortId = vendorDBO.UserShortId;
            
        var vendorHQ = await vendorHQRepository.GetVendorHQAsync(vendorDBO.HQ, vendorDBO.UserShortId.Value);
        if (vendorHQ is not null)
            vendorDBO.HQ = vendorHQ;
        else if (vendorDBO.HQ is not null)
            vendorDBO.HQ.UserShortId = vendorDBO.UserShortId;

        await context.Vendors.AddAsync(vendorDBO);
        await context.SaveChangesAsync();
        return vendorDBO;
    }

    public async Task<VendorDBO> GetOrCreateVendorAsync(VendorDBO vendorDBO) =>
        await GetVendorAsync(vendorDBO, vendorDBO.UserShortId.Value) ??
            await CreateVendorAsync(vendorDBO);

}
