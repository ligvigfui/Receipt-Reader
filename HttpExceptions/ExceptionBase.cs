namespace HttpExceptions;

public abstract class ExceptionBase : Exception
{
    public virtual HttpStatusCode StatusCode { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    public object? DataObject { get; set; }
    public ExceptionBase() { }
    public ExceptionBase(string message) : base(message) { }
    public ExceptionBase(string message, Exception innerException) : base(message, innerException) { }
}
