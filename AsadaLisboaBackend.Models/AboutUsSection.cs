namespace AsadaLisboaBackend.Models;

public class AboutUsSection
{
    public Guid Id { get; set; }
    public string SectionType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public byte Order { get; set; }
}