using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Models.DTOs.Editor;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Editors;

public class EditorsAdderService(
    IFileSystemsManager fileSystemsManager,
    ILogger<EditorsAdderService> logger)
    : IEditorsAdderService
{
    public async Task<EditorResponseDto> CreateTemporalImage(EditorRequestDto editorRequestDto)
    {
        if(editorRequestDto.File == null)
        {
            logger.LogWarning("No se ha proporcionado ningún archivo para crear la imagen temporal.");
            throw new ArgumentException("El archivo no puede ser nulo.");
        }

        var url = await fileSystemsManager.SaveAsync(editorRequestDto.File, "temp");

        logger.LogInformation("Imagen temporal agregada en esta URL: {Url}", url);

        return new EditorResponseDto() { Url = url };
    }
}