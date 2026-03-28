using Microsoft.AspNetCore.Http;

namespace AsadaLisboaBackend.ServiceContracts.FileSystem
{
    public interface IFileSystemManager
    {
        Task<string> SaveAsync(IFormFile file, string folder);
        Task MoveAsync(string fileName, string sourceFolder, string destinationFolder);
        Task DeleteAsync(string fileName, string folder);
        bool Exists(string fileName, string folder);
    }
}
