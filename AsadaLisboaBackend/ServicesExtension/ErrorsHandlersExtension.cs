using AsadaLisboaBackend.ErrorHandling;

namespace AsadaLisboaBackend.ServicesExtension
{
    public static class ErrorsHandlersExtension
    {
        public static IServiceCollection ErrorsHandlersRegistration(this IServiceCollection services)
        {
            services.AddExceptionHandler<InvalidRefreshTokenErrorHandling>();
            services.AddExceptionHandler<InvalidAccessTokenErrorHandling>();
            services.AddExceptionHandler<InvalidCredentialsErrorHandling>();
            services.AddExceptionHandler<SecurityTokenErrorHandling>();
            services.AddExceptionHandler<CreateObjectErrorHandling>();
            services.AddExceptionHandler<RegisterUserErrorHandling>();
            services.AddExceptionHandler<UpdateObjectErrorHandling>();
            services.AddExceptionHandler<ArgumentNullErrorHandling>();
            services.AddExceptionHandler<DbUpdateErrorHandling>();
            services.AddExceptionHandler<ArgumentErrorHandling>();
            services.AddExceptionHandler<NotFoundErrorHandling>();
            services.AddExceptionHandler<IdentityErrorHandling>();
            services.AddExceptionHandler<GlobalErrorHandling>();

            return services;
        }
    }
}
