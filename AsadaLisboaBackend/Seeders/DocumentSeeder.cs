using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Seeders.SeedData;
using AsadaLisboaBackend.Models.DatabaseContext;

namespace AsadaLisboaBackend.Seeders
{
    /// <summary>
    /// Seeds the database with initial document data.
    /// </summary>
    public static class DocumentSeeder
    {
        /// <summary>
        /// Seeds the database with initial document data.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Documents.AnyAsync())
                return;

            var categories = await context.Categories.ToListAsync();
            var documents = DocumentsSeedData.Get(categories);

            await context.Documents.AddRangeAsync(documents);
            await context.SaveChangesAsync();
        }
    }
}
