namespace AsadaLisboaBackend.Models.DTOs.SearchGlobal
{
    public class SearchGlobalDocument
    {
        public string Id { get; set; }
        public string Type { get; set; } // Noticia, Documento, Imagen
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Slug { get; set; }
    }
}
