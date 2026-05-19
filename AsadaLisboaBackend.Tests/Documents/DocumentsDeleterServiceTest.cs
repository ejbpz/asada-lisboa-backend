using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using Elastic.Clients.Elasticsearch;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Documents;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Documents;

namespace AsadaLisboaBackend.Tests.Services.Documents
{
    public class DocumentsDeleterServiceTest
    {
        private readonly Mock<IFileSystemsManager> _fileSystemsMock = new Mock<IFileSystemsManager>();
        private readonly Mock<IMemoryCachesService> _memoryCacheMock = new Mock<IMemoryCachesService>();
        private readonly Mock<IDocumentsGetterRepository> _getterRepositoryMock = new Mock<IDocumentsGetterRepository>();
        private readonly Mock<ILogger<DocumentsDeleterService>> _loggerMock = new Mock<ILogger<DocumentsDeleterService>>();
        private readonly Mock<IDocumentsDeleterRepository> _deleterRepositoryMock = new Mock<IDocumentsDeleterRepository>();

        private readonly Mock<ElasticsearchClient> _elasticMock;

        private readonly DocumentsDeleterService _service;

        public DocumentsDeleterServiceTest()
        {
            _elasticMock = new Mock<ElasticsearchClient>();

            _service = new DocumentsDeleterService(
                _fileSystemsMock.Object,
                _loggerMock.Object,
                _deleterRepositoryMock.Object,
                _getterRepositoryMock.Object,
                _memoryCacheMock.Object,
                _elasticMock.Object
            );
        }

        [Fact]
        public async Task DeleterDocument_ShouldThrowNotFoundException_WhenDocumentDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _getterRepositoryMock
                .Setup(x => x.GetDocument(id))!
                .ReturnsAsync((Document?)null);

            // Act
            Func<Task> action = async () => await _service.DeleterDocument(id);

            // Assert
            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Documento no encontrado.");

            _deleterRepositoryMock.Verify(
                x => x.DeleteDocument(It.IsAny<Guid>()),
                Times.Never);

            _memoryCacheMock.Verify(
                x => x.ChangeVersion(It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleterDocument_ShouldDeleteDocument_WhenDocumentExists()
        {
            // Arrange
            var id = Guid.NewGuid();

            var document = new Document
            {
                Id = id,
                FileName = "test.pdf",
                FilePath = "documentos/test.pdf"
            };

            _getterRepositoryMock
                .Setup(x => x.GetDocument(id))
                .ReturnsAsync(document);

            // Act
            await _service.DeleterDocument(id);

            // Assert
            _getterRepositoryMock.Verify(
                x => x.GetDocument(id),
                Times.Once);

            _deleterRepositoryMock.Verify(
                x => x.DeleteDocument(id),
                Times.Once);

            _memoryCacheMock.Verify(
                x => x.RemoveById(Constants.CACHE_DOCUMENTS, id),
                Times.Once);

            _memoryCacheMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_DOCUMENTS),
                Times.Once);
        }

        [Fact]
        public async Task DeleterDocument_ShouldCallDeleteAsync_WhenFileExists()
        {
            // Arrange
            var id = Guid.NewGuid();

            var tempPath = Path.GetTempFileName();

            var document = new Document
            {
                Id = id,
                FileName = "test.pdf",
                FilePath = tempPath
            };

            _getterRepositoryMock
                .Setup(x => x.GetDocument(id))
                .ReturnsAsync(document);

            // Act
            await _service.DeleterDocument(id);

            // Assert
            _fileSystemsMock.Verify(
                x => x.DeleteAsync("test.pdf", "documentos"),
                Times.Once);

            // Cleanup
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }

        [Fact]
        public async Task DeleterDocument_ShouldNotCallDeleteAsync_WhenFileDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            var document = new Document
            {
                Id = id,
                FileName = "test.pdf",
                FilePath = "path/not-exists.pdf"
            };

            _getterRepositoryMock
                .Setup(x => x.GetDocument(id))
                .ReturnsAsync(document);

            // Act
            await _service.DeleterDocument(id);

            // Assert
            _fileSystemsMock.Verify(
                x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }
    }
}