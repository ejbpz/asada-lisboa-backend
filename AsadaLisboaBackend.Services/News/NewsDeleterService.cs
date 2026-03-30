using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsDeleterService : INewsDeleterService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEditorsDeleterService _editorsDeleterService;
        private readonly INewsDeleterRepository _newsDeleterRepository;

        public NewsDeleterService(INewsDeleterRepository newsDeleterRepository, ApplicationDbContext context, IEditorsDeleterService editorsDeleterService)
        {
            _context = context;
            _editorsDeleterService = editorsDeleterService;
            _newsDeleterRepository = newsDeleterRepository;
        }

        public async Task DeleteNew(Guid id)
        {
            var existingNew = await _context.News
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id);

            if (existingNew is null)
                throw new NotFoundException("La noticia no fue encontrada.");

            await _editorsDeleterService.DeletePrincipalImage(existingNew.FileName);

            await _editorsDeleterService.DeleteContentImages(existingNew.Description);

            await _newsDeleterRepository.DeleteNew(id);
        }
    }
}
