using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Seeders.SeedData;
using AsadaLisboaBackend.Models.DatabaseContext;

namespace AsadaLisboaBackend.Seeders
{
    /// <summary>
    /// Seeds the news data into the database.
    /// </summary>
    public static class NewSeeder
    {
        /// <summary>
        /// Seeds the news data into the database.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.News.AnyAsync())
                return;

            var categories = await context.Categories.ToListAsync();
            var news = NewsSeedData.Get(categories);

            await context.News.AddRangeAsync(news);
            await context.SaveChangesAsync();
        }
    }
}
