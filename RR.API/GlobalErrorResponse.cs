namespace RR.API;

public class GlobalErrorResponse(Exception ex)
{
    public string Type { get; set; } = ex.GetType().Name;
    public IEnumerable<string> Errors { get; set; } =
        ex is ExceptionBase exBase ? exBase.Errors : FlattenExceptionMessages(ex);
    public object? DataObject { get; set; } =
        ex is ExceptionBase exBase ? exBase.DataObject : null;

    public string? StackTrace { get; set; } = IsDevelopment ? ex.ToString() : null;

    public static bool IsDevelopment { private get; set; }

    static List<string> FlattenExceptionMessages(Exception ex)
    {
        List<string> errors = [ex.Message];
        while (ex.InnerException != null)
        {
            errors.Add(ex.Message);
            ex = ex.InnerException;
        }
        return errors;
    }
}