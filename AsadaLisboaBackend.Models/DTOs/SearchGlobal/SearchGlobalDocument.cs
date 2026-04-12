namespace AsadaLisboaBackend.Models.DTOs.SearchGlobal
{
    public class SearchGlobalDocument
    {
        public Guid Id { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Type { get; set; } = string.Empty; // Noticia, Documento, Imagen
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
