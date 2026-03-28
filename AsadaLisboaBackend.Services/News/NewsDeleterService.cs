using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Editor;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsDeleterService : INewsDeleterService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEditorDeleterService _editorDeleterService;
        private readonly INewsDeleterRepository _newsDeleterRepository;

        public NewsDeleterService(INewsDeleterRepository newsDeleterRepository, ApplicationDbContext context, IEditorDeleterService editorDeleterService)
        {
            _context = context;
            _editorDeleterService = editorDeleterService;
            _newsDeleterRepository = newsDeleterRepository;
        }

        public async Task DeleteNew(Guid id)
        {
            var existingNew = await _context.News
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id);

            if (existingNew is null)
                throw new NotFoundException("La noticia no fue encontrada.");

            await _editorDeleterService.DeletePrincipalImage(existingNew.FileName);

            await _editorDeleterService.DeleteContentImages(existingNew.Description);

            await _newsDeleterRepository.DeleteNew(id);
        }
    }
}
