namespace AsadaLisboaBackend.Models;

public class Contact
{
    public Guid Id { get; set; }
    public string ContactType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public byte Order { get; set; }
}