namespace RR.Data.Repository;

class ReceiptRepository(
    ApplicationDbContext context
) : IReceiptRepository
{
    public async Task<ReceiptDBO> CreateReceipt(ReceiptDBO receiptDBO)
    {
        await context.AddAsync(receiptDBO);
        await context.SaveChangesAsync();
        return receiptDBO;
    }
}
