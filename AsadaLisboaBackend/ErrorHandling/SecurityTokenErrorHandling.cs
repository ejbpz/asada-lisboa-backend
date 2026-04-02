using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Diagnostics;

namespace AsadaLisboaBackend.ErrorHandling
{
    internal sealed class SecurityTokenErrorHandling : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public SecurityTokenErrorHandling(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not SecurityTokenException securityTokenException) 
                return false;

            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails()
                {
                    Detail = exception.Message,
                    Title = "Error validación de token",
                    Status = StatusCodes.Status401Unauthorized,
                }
            });

            return true;
        }
    }
}
