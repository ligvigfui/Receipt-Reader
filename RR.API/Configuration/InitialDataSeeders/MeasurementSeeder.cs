namespace RR.API.Configuration.InitialDataSeeders;

public class MeasurementSeeder(ApplicationDbContext context) : ISeedable
{
    public async Task<bool> ShouldSeed() => !await context.Measurments.AnyAsync();
    public async Task Seed()
    {
        await context.Measurments.AddRangeAsync(new List<MeasurementDBO>
            {
                new("Gram", "Grams", "g", MeasurementCategory.Weight, 1),
                new("Liter", "Liters", "L", MeasurementCategory.Volume, 1),
                new("Meter", "Meters", "m", MeasurementCategory.Length, 1),
                new("Piece", "Pieces", "pc", MeasurementCategory.Count, 1),

                new("Kilogram", "Kilograms", "kg", MeasurementCategory.Weight, 1000),
                new("Milliliter", "Milliliters", "mL", MeasurementCategory.Volume, 0.001f),
                new("Centimeter", "Centimeters", "cm", MeasurementCategory.Length, 0.01f),
            });
        await context.SaveChangesAsync();
    }
}