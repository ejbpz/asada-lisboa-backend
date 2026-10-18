namespace AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

public interface IAboutUsSectionsDeleterRepository
{
    public Task DeleteAboutUsSection(Guid id);
}