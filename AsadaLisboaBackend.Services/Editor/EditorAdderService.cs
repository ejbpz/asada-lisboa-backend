using AsadaLisboaBackend.Models.DTOs.Editor;
using AsadaLisboaBackend.ServiceContracts.Editor;
using AsadaLisboaBackend.ServiceContracts.FileSystem;

namespace AsadaLisboaBackend.Services.Editor
{
    public class EditorAdderService : IEditorAdderService
    {
        private IFileSystemManager _fileSystemManager;

        public EditorAdderService(IFileSystemManager fileSystemManager)
        {
            _fileSystemManager = fileSystemManager;
        }

        public async Task<EditorResponseDTO> CreateTemporalImage(EditorRequestDTO editorRequestDTO)
        {
            var url = await _fileSystemManager.SaveAsync(editorRequestDTO.File, "temp");

            return new EditorResponseDTO() { Url = url };
        }
    }
}
