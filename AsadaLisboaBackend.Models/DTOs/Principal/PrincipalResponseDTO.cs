using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.Models.DTOs.Principal;

public class PrincipalResponseDto
{
    public List<NewMinimalResponseDto> News { get; set; } = new List<NewMinimalResponseDto>();
    public List<ImageMinimalResponseDto> Images { get; set; } = new List<ImageMinimalResponseDto>();
    public List<DocumentMinimalResponseDto> Documents { get; set; } = new List<DocumentMinimalResponseDto>();
}