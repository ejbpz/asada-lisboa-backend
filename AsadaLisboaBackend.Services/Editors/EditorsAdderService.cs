using AsadaLisboaBackend.Models.DTOs.Editor;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Editors
{
    public class EditorsAdderService : IEditorsAdderService
    {
        private IFileSystemsManager _fileSystemsManager;

        public EditorsAdderService(IFileSystemsManager fileSystemsManager)
        {
            _fileSystemsManager = fileSystemsManager;
        }

        public async Task<EditorResponseDTO> CreateTemporalImage(EditorRequestDTO editorRequestDTO)
        {
            var url = await _fileSystemsManager.SaveAsync(editorRequestDTO.File, "temp");

            return new EditorResponseDTO() { Url = url };
        }
    }
}
