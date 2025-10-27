using Microsoft.EntityFrameworkCore;
using RR.Common.Models;
using RR.Data;
using RR.Data.DataBaseObjects;
using RR.Data.Repository;

namespace RR.Tests;

public class ProductRepositoryTests
{
    ProductRepository GetProductRepository(IEnumerable<ProductDBO> products)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var context = new ApplicationDbContext(options);
        context.Products.AddRange(products);
        context.SaveChanges();
        return new ProductRepository(null, context);
    }

    [Test]
    public async Task CanReadPublic()
    {
        // Arrange
        const int myUserShortId = 1;
        const int otherUserInMyGroupShortId = 2;
        const int otherUserNotInMyGroupShortId = 3;
        const int myCanReadGroupId = 1;
        const int myCanReadOwnGroupId = 2;
        const int otherGroupId = 3;
        var myCanReadGroup = GroupFromCanReadOptions(true, false, myCanReadGroupId);
        var myCanReadOwnGroup = GroupFromCanReadOptions(false, true, myCanReadOwnGroupId);
        var otherGroup = new GroupDBO()
        {
            Id = otherGroupId,
            Name = "Other Group",
            AreItemsDefaultPublic = false,
            UserGroups =
            [
                new() {
                    UserShortId = otherUserNotInMyGroupShortId,
                    GroupId = otherGroupId,
                    CanRead = true,
                    CanReadOwn = true,
                },
            ],
        };
        static GroupDBO GroupFromCanReadOptions(bool canRead, bool canReadOwn, int groupId)
        {
            return new GroupDBO
            {
                Id = groupId,
                Name = "My Group",
                AreItemsDefaultPublic = false,
                UserGroups =
                [
                    new() {
                        UserShortId = myUserShortId,
                        GroupId = groupId,
                        CanRead = canRead,
                        CanReadOwn = canReadOwn,
                    },
                    new() {
                        UserShortId = otherUserInMyGroupShortId,
                        GroupId = groupId,
                        CanRead = false,
                        CanReadOwn = false,
                    },
                ],
            };
        }
        var ProductSetAndResult = new List<(bool shouldSee, int uploaderUserId, GroupDBO? group)>()
        {
            (true, myUserShortId, null),
            (false, myUserShortId, myCanReadGroup),
            (true, myUserShortId, myCanReadOwnGroup),
            (false, otherUserInMyGroupShortId, null),
            (true, otherUserInMyGroupShortId, myCanReadGroup),
            (false, otherUserInMyGroupShortId, myCanReadOwnGroup),
            (false, otherUserNotInMyGroupShortId, null),
            (false, otherUserNotInMyGroupShortId, otherGroup),
        };

        List<ProductDBO> products = [];
        var productId = 1;
        void CreateProduct(bool shouldSee, int uploaderUserId, GroupDBO? group, bool isPublic)
        {
            products.Add(new ProductDBO()
            {
                Id = productId++,
                Name = shouldSee.ToString(),
                UserShortId = uploaderUserId,
                GroupId = group?.Id,
                Group = group,
                IsPublic = isPublic,
            });
        }
        foreach (var (shouldSee, uploaderUserId, group) in ProductSetAndResult)
            CreateProduct(shouldSee, uploaderUserId, group, false);
        foreach (var (_, uploaderUserId, group) in ProductSetAndResult)
            CreateProduct(true, uploaderUserId, group, true);

        var productRepository = GetProductRepository(products);

        var filter = new SortPageFilter<Product>
        {
        };

        // Act
        var getProducts = await productRepository.GetProductsAsync(new SortPageFilter<Product>(), myUserShortId);

        // Assert
        foreach (var product in getProducts)
        {
            Assert.That(product.Name , Is.EqualTo(true.ToString()), $"Failed for product with Id: {product.Id}");
        }
        Assert.That(getProducts, Has.Count.EqualTo(ProductSetAndResult.Count(x => x.shouldSee) + ProductSetAndResult.Count), "Total count mismatch");
    }
}
