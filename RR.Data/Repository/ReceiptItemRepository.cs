namespace RR.Data.Repository;

public class ReceiptItemRepository(
    IMeasurementRepository measurementRepository,
    IProductAliasRepository productAliasRepository,
    IProductRepository productRepository,
    IGroupRepository groupRepository
) : IReceiptItemRepository
{
    public async Task<List<ReceiptItemDBO>> PreProcessReceiptItemsAsync(IEnumerable<ReceiptItem> receiptItems, ushort languageId, UserDBO user, int? groupId)
    {
        if (receiptItems.Any(ri => ri.Product is null && ri.Name is null))
            throw new BadRequestException("Either Product or Name must be specified for all ReceiptItems.");
        var products = await productRepository.GetProductsWithIdsAsync(receiptItems.Where(ri => ri.Product is not null).Select(ri => ri.Product!).Distinct(), user.ShortId, groupId);
        var productAliases = await productAliasRepository.GetProductAliasesWithNamesAsync(receiptItems.Where(ri => ri.Name is not null).Select(ri => ri.Name!).Distinct(), languageId, user.ShortId, groupId);

        var measurements = new Dictionary<(int id, string name), MeasurementDBO>();
        var measurementsToGet = new List<(int id, string name)>();
        void GetMeasurement(Measurement m)
        {
            if (!measurements.ContainsKey((m.Id, m.Name)))
                return;
            var measurement = products.FirstOrDefault(p => p.MeasurementId == m.Id && p.Measurement!.Name == m.Name)?.Measurement ??
                productAliases.FirstOrDefault(pa => pa.Product.MeasurementId == m.Id && pa.Product.Measurement!.Name == m.Name)?.Product?.Measurement;
            if (measurement is not null)
                measurements[(m.Id, m.Name)] = measurement;
            else
                measurementsToGet.Add((m.Id, m.Name));
        }

        var receiptItemToProductAndAlias = new Dictionary<ReceiptItem, (ProductDBO? product, ProductAliasDBO? alias)>();
        foreach (var receiptItem in receiptItems)
        {
            var product = products.FirstOrDefault(p => p.Name == receiptItem.Product?.Name && p.Id == receiptItem.Product?.Id);
            var productAlias = productAliases.FirstOrDefault(pa => pa.Name == receiptItem.Name && pa.LanguageId == languageId);
            receiptItemToProductAndAlias[receiptItem] =
            (
                product,
                productAlias
            );
            GetMeasurement(receiptItem.Measurement);
            if (product is null && productAlias is null && receiptItem.Product?.Measurement is not null)
                GetMeasurement(receiptItem.Product!.Measurement);
        }
        if (measurementsToGet.Count != 0)
        {
            var fetchedMeasurements = await measurementRepository.GetMeasurements(measurementsToGet.Select(m => m.id).Distinct());
            
            foreach (var measurement in fetchedMeasurements)
                measurements[(measurement.Id, measurement.Name)] = measurement;
        }
        var differentProductsAndAliases = receiptItemToProductAndAlias.Values
            .Where(v => v.product?.Id is not null && v.alias?.Product?.Id is not null && v.product.Id != v.alias.Product.Id)
            .ToList();
        if (differentProductsAndAliases.Count != 0)
            throw new BadRequestException(
                "ReceiptItems contain Product and Name aliases associated with different products: " +
                string.Join(", ", differentProductsAndAliases.Select(v => $"{v.product!.ToIdAndNameString()} and {v.alias!.ToIdAndNameString()}"))
            );
        var result = new List<ReceiptItemDBO>();
        foreach (var receiptItem in receiptItemToProductAndAlias.Keys)
        {
            var (productDBO, productAliasDBO) = receiptItemToProductAndAlias[receiptItem];
            productDBO ??= productAliasDBO != null ?
                productAliasDBO.Product :
                new ProductDBO()
                {
                    Name = receiptItem.Product!.Name ??
                        throw new BadRequestException($"New Product must have a Name for receipt item: {receiptItem.Name}."),
                    GroupId = (await groupRepository.EnsureCanEditOwn(user.ShortId, receiptItem.Product.GroupId))?.GroupId,
                    IsPublic = receiptItem.Product.IsPublic ??
                        (await groupRepository.EnsureCanEditOwn(user.ShortId, receiptItem.Product.GroupId))?.AreItemsDefaultPublic ??
                        user.AreItemsDefaultPublic,
                    Description = receiptItem.Product.Description,
                    Quantity = receiptItem.Product.Quantity,
                    MeasurementId = measurements.GetValueOrDefault((receiptItem.Product.Measurement.Id, receiptItem.Product.Measurement.Name))?.Id ??
                        throw new BadRequestException($"Invalid measurement: {receiptItem.Product.Measurement} specified for new Product {receiptItem.Product.Name}."),
                    ImageUrl = receiptItem.Product.ImageUrl,
                };
            productAliasDBO ??= receiptItem.Name == null ? null :
                new ProductAliasDBO()
                {
                    UserShortId = user.ShortId,
                    GroupId = (await groupRepository.EnsureCanEditOwn(user.ShortId, groupId))?.GroupId,
                    Name = receiptItem.Name!,
                    LanguageId = languageId,
                    ProductId = productDBO!.Id,
                };
            result.Add(
                new ReceiptItemDBO()
                {
                    ProductAlias = productAliasDBO,
                    Product = productDBO,
                    Quantity = receiptItem.Quantity,
                    MeasurementId = measurements.GetValueOrDefault((receiptItem.Measurement.Id, receiptItem.Measurement.Name))?.Id ??
                        throw new BadRequestException($"Invalid measurement: {receiptItem.Measurement} specified for ReceiptItem {receiptItem.Name}."),
                    PricePerQuantity = receiptItem.PricePerQuantity
                }
            );
        }
        return result;
    }
    public async Task<ReceiptItemDBO> CreateReceiptItemAsync(ReceiptItem receiptItem, ushort languageId, int userShortId, int? groupId)
    {
        if (receiptItem.Product is null && receiptItem.Name is null)
            throw new BadRequestException("Either Product or Name must be specified for ReceiptItem.");
        var productDBO = await productRepository.GetProductAsync(receiptItem.Product, userShortId);
        var productAliasDBO = await productAliasRepository.GetProductAliasAsync(receiptItem.Name, languageId, userShortId);
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
                LanguageId = languageId,
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

    public Task<List<ReceiptItemValidated>> ValidateItems(List<ReceiptItemDBO> items)
    {
        throw new NotImplementedException();
    }
}
