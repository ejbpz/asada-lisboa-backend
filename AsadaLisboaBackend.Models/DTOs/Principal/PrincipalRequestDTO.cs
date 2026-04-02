using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.Models.DTOs.Principal
{
    public class PrincipalRequestDTO
    {
        public List<NewMinimalResponseDTO> News { get; set; } = new List<NewMinimalResponseDTO>();
        public List<ImageMinimalResponseDTO> Images { get; set; } = new List<ImageMinimalResponseDTO>();
        public List<DocumentMinimalResponseDTO> Documents { get; set; } = new List<DocumentMinimalResponseDTO>();
    }
}
