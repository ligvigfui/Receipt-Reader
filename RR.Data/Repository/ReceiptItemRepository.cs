namespace RR.Data.Repository;

public class ReceiptItemRepository(
    IMeasurementRepository measurementRepository,
    IProductAliasRepository productAliasRepository,
    IProductRepository productRepository
) : IReceiptItemRepository
{
    public async Task<IEnumerable<ReceiptItemDBO>> CreateReceiptItemAsync(IEnumerable<ReceiptItem> receiptItems, string language, int userShortId, int? groupId)
    {
        if (receiptItems.Any(ri => ri.Product is null && ri.Name is null))
            throw new BadRequestException("Either Product or Name must be specified for all ReceiptItems.");
        var products = await productRepository.GetProductsWithIdsAsync(receiptItems.Select(ri => ri.Product), userShortId);
    }
    public async Task<ReceiptItemDBO> CreateReceiptItemAsync(ReceiptItem receiptItem, string language, int userShortId, int? groupId)
    {
        if (receiptItem.Product is null && receiptItem.Name is null)
            throw new BadRequestException("Either Product or Name must be specified for ReceiptItem.");
        var productDBO = await productRepository.GetProductAsync(receiptItem.Product, userShortId);
        var productAliasDBO = await productAliasRepository.GetProductAliasAsync(receiptItem.Name, language, userShortId);
        if (productDBO?.Id is not null && productAliasDBO?.Id is not null && productDBO.Id != productAliasDBO.Id)
            throw new BadRequestException($"ReceiptItem Product {productDBO.ToIdAndNameString()} and Name alias {productAliasDBO.ToIdAndNameString()} are associated with different products.");

        productDBO ??= productAliasDBO != null ?
            productAliasDBO.Product :
            await productRepository.CreateProductAsync(receiptItem.Product!);
        productAliasDBO ??= receiptItem.Name == null ? null :
            await productAliasRepository.CreateProductAliasAsync(new ProductAliasDBO()
            {
                UserShortId = userShortId,
                GroupId = groupId,
                Name = receiptItem.Name!,
                Language = language,
                ProductId = productDBO!.Id,
            });

        var measurement = await measurementRepository.GetMeasurement(receiptItem.Measurement) ??
            throw new BadRequestException($"Invalid measurement: {receiptItem.Measurement} specified for ReceiptItem {receiptItem.Name}.");
        var receiptItemDBO = new ReceiptItemDBO
        {
            ProductAliasId = productAliasDBO?.Id,
            ProductId = productDBO.Id,
            Quantity = receiptItem.Quantity,
            MeasurementId = measurement.Id,
            PricePerQuantity = receiptItem.PricePerQuantity
        };
        return receiptItemDBO;
    }
}
