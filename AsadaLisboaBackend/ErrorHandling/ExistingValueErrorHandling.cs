using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class ExistingValueErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<ExistingValueErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ExistingValueException existingValueException)
            return false;

        logger.LogError(exception, "Error Global - Error al utilizar un valor ya existente.");

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Nombre del valor utilizado",
                Status = StatusCodes.Status400BadRequest,
            }
        });

        return true;
    }
}