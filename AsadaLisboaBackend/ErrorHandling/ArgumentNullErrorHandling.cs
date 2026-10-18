using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class ArgumentNullErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<ArgumentNullErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ArgumentNullException)
            return false;

        logger.LogError(exception, "Error Global - Error al enviar un valor nulo.");

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Nulo en el valor enviado",
                Status = StatusCodes.Status400BadRequest,
            }
        });

        return true;
    }
}