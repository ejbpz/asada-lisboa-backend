using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Repositories.News;

public class NewsDeleterRepository(ApplicationDbContext context) : INewsDeleterRepository
{
    public async Task DeleteNew(Guid id)
    {
        var affectedRows = await context.News
            .Where(n => n.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows < 1)
            throw new NotFoundException("La noticia no fue encontrada.");
    }
}