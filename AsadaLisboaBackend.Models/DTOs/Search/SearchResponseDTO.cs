namespace AsadaLisboaBackend.Models.DTOs.Search
{
    public class SearchResponseDTO
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Noticia, Documento, Imagen
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
