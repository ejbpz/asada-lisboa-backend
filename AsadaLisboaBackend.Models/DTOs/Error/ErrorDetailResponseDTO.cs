namespace AsadaLisboaBackend.Models.DTOs.Error;

public class ErrorDetailResponseDto
{
    public string Code { get; init; }
    public string Message { get; init; }

    public ErrorDetailResponseDto(string code, string message)
    {
        Code = code;
        Message = message;
    }
}