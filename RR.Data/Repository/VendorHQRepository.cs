namespace RR.Data.Repository;

public class VendorHQRepository(
    IAddressRepository addressRepository,
    IGroupRepository groupRepository,
    ApplicationDbContext context
) : IVendorHQRepository
{
    public async Task<VendorHQDBO?> GetVendorHQAsync(VendorHQ? vendorHQ, int userShortId) => vendorHQ is null ? null :
        await context.VendorHQs.WhereCanRead(userShortId).Include(vh => vh.Address).FirstOrDefaultAsync(vh => vh.Id == vendorHQ.Id && vh.Name == vendorHQ.Name);
    public async Task<VendorHQDBO> CreateVendorHQAsync(VendorHQDBO vendorHQDBO)
    {
        await groupRepository.EnsureCanEditOwn(vendorHQDBO.GroupId, vendorHQDBO.UserShortId);
        var address = await addressRepository.GetAddressAsync(vendorHQDBO.Address, vendorHQDBO.UserShortId.Value);
        if (address is not null)
            vendorHQDBO.Address = address;
        else if (vendorHQDBO.Address is not null)
            vendorHQDBO.Address.UserShortId = vendorHQDBO.UserShortId;
        await context.VendorHQs.AddAsync(vendorHQDBO);
        await context.SaveChangesAsync();
        return vendorHQDBO;
    }
    public async Task<VendorHQDBO> CreateOrGetVendorHQAsync(VendorHQDBO vendorHQDBO) =>
        await GetVendorHQAsync(vendorHQDBO, vendorHQDBO.UserShortId.Value) ??
            await CreateVendorHQAsync(vendorHQDBO);
}
