namespace RR.Common.Models;

public class SortPageFilter<F>
{
    public List<Sort>? Sorts { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public F? Filter { get; set; }
    public void Validate()
    {
        var filterProperties = typeof(F).GetProperties().Select(p => p.Name.ToLower());
        if (Sorts != null && Sorts.Any(s => !filterProperties.Contains(s.SortByField.ToLower())))
            throw new ArgumentException("One or more SortByField values are not valid properties of the filter type.");
    }
}
public class Sort
{
    public required string SortByField { get; set; }
    public SortDirection SortDirection { get; set; }
}