namespace HttpExceptions;

public class NotFoundException : ExceptionBase
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    public NotFoundException() { }
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}