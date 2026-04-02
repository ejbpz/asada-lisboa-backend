using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;

namespace AsadaLisboaBackend.ErrorHandling
{
    internal sealed class DbUpdateErrorHandling : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public DbUpdateErrorHandling(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not DbUpdateException dbUpdateException || exception is not DbUpdateConcurrencyException dbUpdateConcurrencyException)
                return false;

            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

            await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails()
                {
                    Detail = exception.Message,
                    Title = "Error al actualizar el elemento",
                    Status = StatusCodes.Status409Conflict,
                }
            });

            return true;
        }
    }
}
