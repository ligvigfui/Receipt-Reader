

namespace RR.Data.Interfaces;

public interface IAddressRepository
{
    Task<AddressDBO> CreateAddressAsync(AddressDBO address);
    Task<AddressDBO?> GetAddressAsync(Address address, int userShortId);
    Task<AddressDBO> GetOrCreateAddressAsync(AddressDBO addressDBO);
}
