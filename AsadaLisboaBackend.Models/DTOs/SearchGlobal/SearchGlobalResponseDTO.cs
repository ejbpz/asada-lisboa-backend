namespace AsadaLisboaBackend.Models.DTOs.SearchGlobal
{
    public class SearchGlobalResponseDTO
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty; // News, Document, Image

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public DateTime Date { get; set; }
    }
}
