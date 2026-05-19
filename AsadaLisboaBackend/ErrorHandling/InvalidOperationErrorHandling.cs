using Microsoft.AspNetCore.Mvc;

namespace AsadaLisboaBackend.ErrorHandling
{
    internal class InvalidOperationErrorHandling
    {
        private readonly ILogger<InvalidOperationErrorHandling> _logger;
        private readonly IProblemDetailsService _problemDetailsService;

        public InvalidOperationErrorHandling(IProblemDetailsService problemDetailsService, ILogger<InvalidOperationErrorHandling> logger)
        {
            _logger = logger;
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not InvalidOperationException invalidOperationException)
                return false;

            _logger.LogError(exception, "Error Global - Error al realizar una operación.");

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
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
}
