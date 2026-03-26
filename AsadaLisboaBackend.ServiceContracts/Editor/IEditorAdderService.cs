using AsadaLisboaBackend.Models.DTOs.Editor;
using AsadaLisboaBackend.Utils.OptionsPattern;

namespace AsadaLisboaBackend.ServiceContracts.Editor
{
    public interface IEditorAdderService
    {
        public Task<EditorResponseDTO> CreateTemporalImage(EditorRequestDTO editorRequestDTO, FileStorageOptions options);
    }
}
