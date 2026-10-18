namespace AsadaLisboaBackend.Models;

public class DocumentType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
}