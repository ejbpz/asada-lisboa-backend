using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Images;

namespace AsadaLisboaBackend.Repositories.Images;

public class ImagesDeleterRepository(ApplicationDbContext context) : IImagesDeleterRepository
{
    public async Task DeleteImage(Guid id)
    {
        var affectedRows = await context.Images
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows < 1)
            throw new NotFoundException("Error al eliminar la imagen.");
    }
}