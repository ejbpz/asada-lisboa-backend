using Microsoft.AspNetCore.Http;

namespace AsadaLisboaBackend.ServiceContracts.FileSystems
{
    public interface IFileSystemsManager
    {
        Task<string> SaveAsync(IFormFile file, string folder);
        Task MoveAsync(string fileName, string sourceFolder, string destinationFolder);
        Task DeleteAsync(string fileName, string folder);
        bool Exists(string fileName, string folder);
    }
}
