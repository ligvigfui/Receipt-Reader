namespace RR.Data.Interfaces;

public interface IAddressRepository
{
    Task<AddressDBO> CreateAddressAsync(AddressDBO address);
    Task<AddressDBO?> GetAddressByIdAsync(int addressId);
}
