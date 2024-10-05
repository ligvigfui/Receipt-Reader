namespace HttpExceptions;

public class UnauthorizedException : ExceptionBase
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    public UnauthorizedException() { }
    public UnauthorizedException(string message) : base(message) { }
    public UnauthorizedException(string message, Exception innerException) : base(message, innerException) { }
}