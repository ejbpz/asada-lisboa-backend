using AsadaLisboaBackend.Models.DTOs.Editor;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.ServiceContracts.Editor;

namespace AsadaLisboaBackend.Services.Editor
{
    public class EditorAdderService : IEditorAdderService
    {
        public async Task<EditorResponseDTO> CreateTemporalImage(EditorRequestDTO editorRequestDTO, FileStorageOptions options)
        {
            if (editorRequestDTO.File is null || editorRequestDTO.File.Length == 0)
                throw new ArgumentException("Archivo inválido.");

            if(!Directory.Exists(options.BasePath))
                Directory.CreateDirectory(options.BasePath);

            var extension = Path.GetExtension(editorRequestDTO.File.FileName);
            var fileName = $"{Guid.NewGuid().ToString()}{extension}";
            var fileNameWithPath = Path.Combine(options.BasePath, fileName);

            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await editorRequestDTO.File.CopyToAsync(stream);

            return new EditorResponseDTO()
            {
                Url = $"{options.BaseUrl}/{fileName}"
            };
        }
    }
}
