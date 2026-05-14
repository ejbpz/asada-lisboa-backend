using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DatabaseContext;

namespace AsadaLisboaBackend.Seeders
{
    /// <summary>
    /// Seed the database with initial data for the application.
    /// </summary>
    public static class ApplicationDbSeeder
    {
        /// <summary>
        /// Seeds the database with initial data.
        /// </summary>
        /// <param name="services">The service provider.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            await DocumentSeeder.SeedAsync(context);
            await ImageSeeder.SeedAsync(context);
        }
    }
}
