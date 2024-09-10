namespace RR.API;

public class GlobalErrorResponse(Exception? ex)
{
    public string Type { get; set; } = ex?.GetType().Name ?? "";
    public string Message { get; set; } = GetMessages(ex) ?? "";
    public string StackTrace { get; set; } = IsDevelopment && ex is not null ? ex.ToString() : "Internal server error";

    public static bool IsDevelopment { private get; set; }

    private static string GetMessages(Exception? ex)
    {
        if (ex is null)
            return "";

        if (ex.InnerException is null)
            return ex.Message;

        return $"{ex.Message} {GetMessages(ex.InnerException)}";
    }
}