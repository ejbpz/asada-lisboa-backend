namespace AsadaLisboaBackend.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public long FileSize { get; set; }

        public string Url { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;

        // Foreign Key
        public Guid StatusId { get; set; }
        public Status? Status { get; set; }
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
