using AsadaLisboaBackend.Models.DTOs.Editor;

namespace AsadaLisboaBackend.ServiceContracts.Editor
{
    public interface IEditorAdderService
    {
        public Task<EditorResponseDTO> CreateTemporalImage(EditorRequestDTO editorRequestDTO);
    }
}
