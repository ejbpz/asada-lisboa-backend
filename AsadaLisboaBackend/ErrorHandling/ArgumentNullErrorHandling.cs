using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AsadaLisboaBackend.ErrorHandling
{
    internal sealed class ArgumentNullErrorHandling : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public ArgumentNullErrorHandling(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not ArgumentNullException argumentNullException)
                return false;

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
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
}
