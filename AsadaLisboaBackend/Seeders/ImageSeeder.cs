using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Seeders.SeedData;
using AsadaLisboaBackend.Models.DatabaseContext;

namespace AsadaLisboaBackend.Seeders
{
    /// <summary>
    /// Seeds the database with initial image data.
    /// </summary>
    public class ImageSeeder
    {
        /// <summary>
        /// Seeds the database with initial image data.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Images.AnyAsync())
                return;

            var images = ImagesSeedData.Get();

            await context.Images.AddRangeAsync(images);

            await context.SaveChangesAsync();
        }
    }
}
