using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Extension method to seed admin user.
    /// </summary>
    public static class SeedAdminUserExtension
    {
        /// <summary>
        /// Seed admin user apply to service provider.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <returns>List of registered services.</returns>
        /// <exception cref="CreateObjectException">To create user and assign roles.</exception>
        public static async Task SeedAdminUserAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var defaultUserOptions = configuration
                .GetSection("DefaultUserOptions")
                .Get<DefaultUserOptions>();

            var email = defaultUserOptions?.EMAIL;
            var password = defaultUserOptions?.PASSWORD;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return;

            var existsUser = await userManager.FindByEmailAsync(email);

            if (existsUser is not null)
                return;

            var user = new ApplicationUser
            {
                Id = Guid.Parse("25EBE143-8D68-42F3-B796-E15F89A65AF5"),
                ChargeId = Guid.Parse("0FEB0B33-CF81-4973-9754-E45DE997069D"),
                Email = email,
                UserName = email,
                FirstName = "Administrador",
                FirstLastName = "ASADA",
                SecondLastName = "Lisboa",
                IsActive = true,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new CreateObjectException($"Error creando usuario admin: {errors}");
            }

            var roleResult = await userManager.AddToRoleAsync(user, "Administrador");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new CreateObjectException($"Error asignando rol: {errors}");
            }
        }
    }
}
