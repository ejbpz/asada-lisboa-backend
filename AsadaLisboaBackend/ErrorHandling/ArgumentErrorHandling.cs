using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace AsadaLisboaBackend.ErrorHandling
{
    internal sealed class ArgumentErrorHandling : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public ArgumentErrorHandling(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not ArgumentException argumentException)
                return false;

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails()
                {
                    Detail = exception.Message,
                    Title = "Error en el valor enviado",
                    Status = StatusCodes.Status400BadRequest,
                }
            });
        }
    }
}
