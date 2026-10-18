using AsadaLisboaBackend.Models.DTOs.Editor;

namespace AsadaLisboaBackend.ServiceContracts.Editors;

public interface IEditorsAdderService
{
    public Task<EditorResponseDto> CreateTemporalImage(EditorRequestDto editorRequestDto);
}