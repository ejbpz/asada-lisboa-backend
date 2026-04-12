using AsadaLisboaBackend.ErrorHandling;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Global error handlers extension method.
    /// </summary>
    public static class ErrorsHandlersExtension
    {
        /// <summary>
        /// Registration of global error handlers into services.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <returns>List of registered services.</returns>
        public static IServiceCollection ErrorsHandlersRegistration(this IServiceCollection services)
        {
            services.AddExceptionHandler<InvalidRefreshTokenErrorHandling>();
            services.AddExceptionHandler<InvalidAccessTokenErrorHandling>();
            services.AddExceptionHandler<InvalidCredentialsErrorHandling>();
            services.AddExceptionHandler<SecurityTokenErrorHandling>();
            services.AddExceptionHandler<ExistingValueErrorHandling>();
            services.AddExceptionHandler<CreateObjectErrorHandling>();
            services.AddExceptionHandler<RegisterUserErrorHandling>();
            services.AddExceptionHandler<UpdateObjectErrorHandling>();
            services.AddExceptionHandler<ArgumentNullErrorHandling>();
            services.AddExceptionHandler<DbUpdateErrorHandling>();
            services.AddExceptionHandler<ArgumentErrorHandling>();
            services.AddExceptionHandler<NotFoundErrorHandling>();
            services.AddExceptionHandler<IdentityErrorHandling>();
            services.AddExceptionHandler<InUsedErrorHandling>();
            services.AddExceptionHandler<GlobalErrorHandling>();

            return services;
        }
    }
}
