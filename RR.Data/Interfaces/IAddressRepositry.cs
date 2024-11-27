namespace RR.Data.Interfaces;

public interface IAddressRepository
{
    Task<Address> CreateAddressAsync(AddressDBO address);
    Task<Address?> GetAddressByIdAsync(int addressId);
}
