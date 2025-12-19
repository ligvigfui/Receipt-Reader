namespace RR.Data.Interfaces;

public interface IMeasurementRepository
{
    Task<MeasurementDBO> CreateMeasurement(CreateMeasurement measurement);
    Task<MeasurementDBO?> GetMeasurement(Measurement? measurement);
    Task<List<MeasurementDBO>> GetMeasurements(IEnumerable<int> measurementIds);
    Task<List<MeasurementDBO>> GetMeasurements(IEnumerable<string> measurementSymbols);
}
