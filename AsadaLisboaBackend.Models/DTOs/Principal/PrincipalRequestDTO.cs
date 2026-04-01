using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.Models.DTOs.Principal
{
    public class PrincipalRequestDTO
    {
        public List<NewResponseDTO> News { get; set; } = new List<NewResponseDTO>();
        public List<ImageResponseDTO> Images { get; set; } = new List<ImageResponseDTO>();
        public List<DocumentResponseDTO> Documents { get; set; } = new List<DocumentResponseDTO>();
    }
}
