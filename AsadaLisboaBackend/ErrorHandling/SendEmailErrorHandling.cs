using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class SendEmailErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<SendEmailErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not SendEmailException)
            return false;

        logger.LogError(exception, "Error Global - Error al enviar un correo electrónico.");

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Error al enviar correo electrónico",
                Status = StatusCodes.Status400BadRequest,
            }
        });

        return true;
    }
}