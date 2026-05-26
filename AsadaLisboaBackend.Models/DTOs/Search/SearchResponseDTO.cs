namespace AsadaLisboaBackend.Models.DTOs.Search
{
    public class SearchResponseDTO
    {
        public Guid Id { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Type { get; set; } = string.Empty; // News, Document, Image
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
