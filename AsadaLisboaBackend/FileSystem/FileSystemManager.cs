using AsadaLisboaBackend.ServiceContracts.FileSystem;

namespace AsadaLisboaBackend.FileSystem
{
    public class FileSystemManager : IFileSystemManager
    {
        private readonly IWebHostEnvironment _env;

        public FileSystemManager(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task DeleteAsync(string fileName, string folder)
        {
            var path = Path.Combine(GetPhysicalPath(folder), fileName);

            if(File.Exists(path))
                File.Delete(path);

            return Task.CompletedTask;
        }

        public bool Exists(string fileName, string folder)
        {
            var path = Path.Combine(GetPhysicalPath(folder), fileName);
            return File.Exists(path);
        }

        public Task MoveAsync(string fileName, string sourceFolder, string destinationFolder)
        {
            var sourcePath = Path.Combine(GetPhysicalPath(sourceFolder), fileName);
            var destinationPath = Path.Combine(GetPhysicalPath(destinationFolder), fileName);

            if (!File.Exists(sourcePath))
                throw new FileNotFoundException("Archivo no encontrado.", sourcePath);

            var destinationPhysicalFolder = GetPhysicalPath(destinationFolder);

            if (!Directory.Exists(destinationPhysicalFolder))
                Directory.CreateDirectory(destinationPhysicalFolder);

            if (File.Exists(destinationPath))
                File.Delete(destinationPath);

            File.Move(sourcePath, destinationPath);

            return Task.CompletedTask;
        }

        public async Task<string> SaveAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Archivo inválido.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extension}";

            var physicalFolder = GetPhysicalPath(folder);

            if(!Directory.Exists(physicalFolder))
                Directory.CreateDirectory(physicalFolder);

            var fullPath = Path.Combine(physicalFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return BuildUrl(folder, fileName);
        }

        private string GetPhysicalPath(string folder)
        {
            return Path.Combine(_env.WebRootPath, folder);
        }

        private string BuildUrl(string folder, string fileName)
        {
            return $"/{folder}/{fileName}";
        }
    }
}
