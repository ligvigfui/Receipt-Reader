namespace RR.Data.DataBaseObjects;

[Tables(nameof(MeasurementDBO))]
public class MeasurementDBO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Plural { get; set; }
    public string Symbol { get; set; }
    public MeasurementCategory Category { get; set; }
    public float ConversionFactorToSI { get; set; }

    public MeasurementDBO() { }
    public MeasurementDBO(string name, string plural, string symbol, MeasurementCategory category, float conversionFactorToSI)
    {
        Name = name;
        Plural = plural;
        Symbol = symbol;
        Category = category;
        ConversionFactorToSI = conversionFactorToSI;
    }
    public static implicit operator Measurement?(MeasurementDBO? measurementDBO) => measurementDBO is null ? null : new()
    {
        Id = measurementDBO.Id,
        Name = measurementDBO.Name,
        Plural = measurementDBO.Plural,
        Symbol = measurementDBO.Symbol,
        Category = measurementDBO.Category,
        ConversionFactorToSI = measurementDBO.ConversionFactorToSI,
    };
}