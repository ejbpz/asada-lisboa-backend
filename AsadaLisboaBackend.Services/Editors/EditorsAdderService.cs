using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Models.DTOs.Editor;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Editors
{
    public class EditorsAdderService : IEditorsAdderService
    {
        private IFileSystemsManager _fileSystemsManager;
        private readonly ILogger<EditorsAdderService> _logger;

        public EditorsAdderService(IFileSystemsManager fileSystemsManager, ILogger<EditorsAdderService> logger)
        {
            _logger = logger;
            _fileSystemsManager = fileSystemsManager;
        }

        public async Task<EditorResponseDTO> CreateTemporalImage(EditorRequestDTO editorRequestDTO)
        {
            if(editorRequestDTO.File == null)
            {
                _logger.LogWarning("No se ha proporcionado ningún archivo para crear la imagen temporal.");
                throw new ArgumentException("El archivo no puede ser nulo.", nameof(editorRequestDTO.File));
            }

            var url = await _fileSystemsManager.SaveAsync(editorRequestDTO.File, "temp");

            _logger.LogInformation("Imagen temporal agregada en esta URL: {Url}", url);

            return new EditorResponseDTO() { Url = url };
        }
    }
}
