namespace HttpExceptions;

public class BadRequestException : ExceptionBase
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    public BadRequestException() { }
    public BadRequestException(string message) : base(message) { }
    public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
}