using Microsoft.AspNetCore.Diagnostics;

namespace RR.API.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController(ILogger<ErrorsController> logger) : ControllerBase
{
    [Route("error")]
    public GlobalErrorResponse Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        
        var exception = context!.Error;

        var code = exception switch
        {
            ExceptionBase httpException => httpException.StatusCode,
            _ => HttpStatusCode.InternalServerError
        };

        Response.StatusCode = (int)code;

        logger.LogError(exception, exception.ToString(), code);

        return new GlobalErrorResponse(exception);
    }
}
