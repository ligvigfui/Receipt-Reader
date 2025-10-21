namespace RR.Service.Interfaces;

public interface IReceiptService
{
    Task<Receipt> CreateReceiptAsync(Receipt receipt);
}
