namespace RR.API.Configuration.InitialDataSeeders;

public class CategorySeeder(
    ApplicationDbContext context
) : ISeedable
{
    public async Task<bool> ShouldSeed() => !await context.Categories.AnyAsync();

    public Task Seed()
    {
        context.Categories.Add(
            new CategoryDBO("Root", true,
                [new CategoryDBO("Type", true,
                    [new CategoryDBO("Consumable", true,
                        [new CategoryDBO("Food", true,
                            [new CategoryDBO("Pastry", true,
                                [new CategoryDBO("Bread",
                                    [new CategoryDBO("White")]
                                )]
                            ),
                            new CategoryDBO("Dairy", true,
                                [new CategoryDBO("Milk",
                                    [new CategoryDBO("UHT")]
                                )]
                            )]
                        )]
                    )]
                )]
            )
        );
        return context.SaveChangesAsync();
    }
}
