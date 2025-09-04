namespace RR.Service.Interfaces;

public interface IReceiptService
{
    Task<ReceiptDBO> CreateReceiptAsync(Receipt receipt, string? groupName = null);
}
