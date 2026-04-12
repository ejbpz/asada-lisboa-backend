namespace AsadaLisboaBackend.Models.DTOs.SearchGlobal
{
    public class SearchGlobalRequestDTO
    {
        public string Search { get; set; } = string.Empty;
        public string Tipo { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
