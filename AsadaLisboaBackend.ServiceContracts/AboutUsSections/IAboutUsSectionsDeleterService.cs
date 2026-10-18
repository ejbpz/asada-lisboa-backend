namespace AsadaLisboaBackend.ServiceContracts.AboutUsSections;

public interface IAboutUsSectionsDeleterService
{
    public Task DeleteAboutUsSection(Guid id);
}