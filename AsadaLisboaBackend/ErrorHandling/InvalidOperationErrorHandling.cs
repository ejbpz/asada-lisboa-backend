using Microsoft.AspNetCore.Mvc;

namespace AsadaLisboaBackend.ErrorHandling;

internal class InvalidOperationErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<InvalidOperationErrorHandling> logger)
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InvalidOperationException)
            return false;

        logger.LogError(exception, "Error Global - Error al realizar una operación.");

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Error al realizar la operación",
                Status = StatusCodes.Status400BadRequest,
            }
        });

        return true;
    }
}