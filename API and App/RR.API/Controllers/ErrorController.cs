using Microsoft.AspNetCore.Diagnostics;

namespace RR.API.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController(ILogger<ErrorsController> logger) : ControllerBase
{
    [Route("error")]
    public GlobalErrorResponse Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context?.Error;
        var code = 500;

        if (exception is BadHttpRequestException)
            code = 400;

        Response.StatusCode = code;

        logger.LogError(exception, exception!.ToString(), code);

        return new GlobalErrorResponse(exception);
    }
}