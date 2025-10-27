namespace RR.Data.Repository;

public class AddressRepository(
    IGroupRepository groupRepository,
    ApplicationDbContext context
) : IAddressRepository
{
    public async Task<AddressDBO?> GetAddressAsync(Address? address, int userShortId) => address is null ? null :
        await context.Addresses.WhereCanRead(userShortId).FirstOrDefaultAsync(a => a.Id == address.Id && a.StreetAddress == address.StreetAddress);

    public async Task<AddressDBO> CreateAddressAsync(AddressDBO address)
    {
        await groupRepository.EnsureCanEditOwn(address.GroupId, address.UserShortId);
        await context.Addresses.AddAsync(address);
        await context.SaveChangesAsync();
        return address;
    }

    public async Task<AddressDBO> GetOrCreateAddressAsync(AddressDBO addressDBO) =>
        await GetAddressAsync(addressDBO, addressDBO.UserShortId.Value) ??
            await CreateAddressAsync(addressDBO);
}
