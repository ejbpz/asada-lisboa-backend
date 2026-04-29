namespace AsadaLisboaBackend.Models.DTOs.SearchGlobal
{
    public class SearchGlobalRequestDTO
    {
        public string Query { get; set; } = string.Empty;

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Type { get; set; }
    }
}
