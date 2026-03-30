using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.ErrorHandling
{
    internal sealed class SendEmailErrorHandling : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public SendEmailErrorHandling(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not SendEmailException sendEmailException)
                return false;

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
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
        }
    }
}
