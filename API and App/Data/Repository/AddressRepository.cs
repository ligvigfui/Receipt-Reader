namespace RR.Data.Repository;

public class AddressRepository(ApplicationDbContext context)
{
    public async Task<int> CreateAddressAsync(AddressDBO address)
    {
        await context.Addresses.AddAsync(address);
        await context.SaveChangesAsync();
        return address.Id;
    }

}
