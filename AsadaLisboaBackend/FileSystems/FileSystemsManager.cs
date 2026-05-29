using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.FileSystems
{
    /// <summary>
    /// Service class to manage file system.
    /// </summary>
    public class FileSystemsManager : IFileSystemsManager
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileSystemsManager> _logger;

        /// <summary>
        /// Constructor for FileSystemsManager.
        /// </summary>
        /// <param name="env">Environment variables from the system.</param>
        /// <param name="logger">Trace user excecutions.</param>
        public FileSystemsManager(IWebHostEnvironment env, ILogger<FileSystemsManager> logger)
        {
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// Delete a file from a folder.
        /// </summary>
        /// <param name="fileName">Name of the file, with extension.</param>
        /// <param name="folder">Folder where the file is saved.</param>
        /// <returns>No content.</returns>
        public Task DeleteAsync(string fileName, string folder)
        {
            var path = Path.Combine(GetPhysicalPath(folder), fileName);

            if(File.Exists(path))
                File.Delete(path);

            _logger.LogInformation("Archivo eliminado: {FilePath}", path);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Check if the file exists.
        /// </summary>
        /// <param name="fileName">Name of the file, with extension.</param>
        /// <param name="folder">Folder where the file is saved.</param>
        /// <returns>True or false, depends on the file in the folder.</returns>
        public bool Exists(string fileName, string folder)
        {
            var path = Path.Combine(GetPhysicalPath(folder), fileName);
            return File.Exists(path);
        }

        /// <summary>
        /// Moves a file from a folder to another.
        /// </summary>        
        /// <param name="fileName">Name of the file, with extension.</param>
        /// <param name="sourceFolder">Folder where the file is saved.</param>
        /// <param name="destinationFolder">Folder where the file is going to be.</param>
        /// <returns>No content.</returns>
        /// <exception cref="FileNotFoundException">Exception when the file is not found.</exception>
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
            _logger.LogInformation("Archivo movido de: {SourcePath} a {DestinationPath}", sourcePath, destinationPath);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Save a file in an specific folder.
        /// </summary>
        /// <param name="file">File to be saved.</param>
        /// <param name="folder">Folder where the files is going to be saved.</param>
        /// <param name="customName">Slug or a custom name to the file.</param>
        /// <returns>File new url.</returns>
        /// <exception cref="ArgumentException">File given is not valid.</exception>
        public async Task<string> SaveAsync(IFormFile file, string folder, string? customName = null)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Archivo inválido.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = string.IsNullOrWhiteSpace(customName)
                ? $"{Guid.NewGuid()}{extension}"
                : $"{customName}{extension}";

            var physicalFolder = GetPhysicalPath(folder);

            if(!Directory.Exists(physicalFolder))
                Directory.CreateDirectory(physicalFolder);

            var fullPath = Path.Combine(physicalFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = BuildUrl(folder, fileName);
            _logger.LogInformation("Archivo almacenado en: {Url}", url);

            return url;
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
