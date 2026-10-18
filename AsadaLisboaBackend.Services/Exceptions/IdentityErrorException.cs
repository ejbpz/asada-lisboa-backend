using AsadaLisboaBackend.Models.DTOs.Error;

namespace AsadaLisboaBackend.Services.Exceptions;

public class IdentityErrorException : Exception
{   
    public IReadOnlyCollection<ErrorDetailResponseDto> Errors { get; }
    
    public IdentityErrorException(List<ErrorDetailResponseDto>? errors) { Errors = errors ?? []; }
    public IdentityErrorException(string message, List<ErrorDetailResponseDto>? errors) : base(message) { Errors = errors ?? []; }
    public IdentityErrorException(string message, Exception inner, List<ErrorDetailResponseDto>? errors) : base(message, inner) { Errors = errors ?? []; }
}