namespace RR.Data.Interfaces;

public interface IMeasurementRepository
{
    Task<MeasurementDBO> CreateMeasurement(CreateMeasurement measurement);
    Task<MeasurementDBO?> GetMeasurement(Measurement? measurement);
}
