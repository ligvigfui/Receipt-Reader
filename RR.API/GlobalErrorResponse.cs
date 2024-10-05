namespace RR.API;

public class GlobalErrorResponse(Exception? ex)
{
    public string? Type { get; set; } = ex?.GetType().Name;
    public IEnumerable<string>? Errors { get; set; } =
        ex is not null && ex is ExceptionBase exBase ? exBase.Errors : null;
    public object? DataObject { get; set; } =
        ex is not null && ex is ExceptionBase exBase ? exBase.DataObject : null;

    public string? StackTrace { get; set; } = IsDevelopment ? ex?.ToString() : null;

    public static bool IsDevelopment { private get; set; }
}