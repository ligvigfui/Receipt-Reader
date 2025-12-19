namespace RR.Data.Repository;

public class MeasurementRepository(
    ApplicationDbContext context
) : IMeasurementRepository
{
    public async Task<MeasurementDBO> CreateMeasurement(CreateMeasurement measurement)
    {
        var measurementDBO = new MeasurementDBO
        (
            measurement.Name,
            measurement.Plural,
            measurement.Symbol,
            measurement.Category,
            measurement.ConversionFactorToSI
        );
        context.Measurments.Add(measurementDBO);
        await context.SaveChangesAsync();
        return measurementDBO;
    }

    public async Task<MeasurementDBO?> GetMeasurement(Measurement? measurement) => measurement is null ? null :
        await context.Measurments.FirstOrDefaultAsync(m => m.Id == measurement.Id && m.Name == measurement.Name);

    public async Task<List<MeasurementDBO>> GetMeasurements(IEnumerable<int> measurementIds) =>
        await context.Measurments.Where(m => measurementIds.Contains(m.Id)).ToListAsync();
    public async Task<List<MeasurementDBO>> GetMeasurements(IEnumerable<string> measurementSymbols) =>
        await context.Measurments.Where(m => measurementSymbols.Contains(m.Symbol.ToLower())).ToListAsync();
}
