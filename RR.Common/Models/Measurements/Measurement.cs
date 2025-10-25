namespace RR.Common.Models.Measurements;

public class Measurement
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Plural { get; set; }
    public string? Symbol { get; set; }
    public MeasurementCategory? Category { get; set; }
    public float? ConversionFactorToSI { get; set; }
}
