using AsadaLisboaBackend.Models.DTOs.New;

namespace AsadaLisboaBackend.ServiceContracts.News;

public interface INewsUpdaterService
{
    public Task<NewResponseDto> UpdateNew(Guid id, NewRequestDto newRequestDto);
}