namespace RR.Data.Repository;

public class AddressRepository(ApplicationDbContext context) : IAddressRepository
{
    public async Task<Address> CreateAddressAsync(AddressDBO address)
    {
        await context.Addresses.AddAsync(address);
        await context.SaveChangesAsync();
        return address;
    }

    public async Task<Address?> GetAddressByIdAsync(int addressId) =>
        await context.Addresses.FirstOrDefaultAsync(address => address.Id == addressId);
}
