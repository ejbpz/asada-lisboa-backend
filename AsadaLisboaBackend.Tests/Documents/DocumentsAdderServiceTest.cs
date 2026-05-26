using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Documents;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.RepositoryContracts.DocumentTypes;

namespace AsadaLisboaBackend.Tests.Services.Documents
{
    public class DocumentsAdderServiceTest
    {
        private readonly Fixture _fixture = new();

        private readonly Mock<IFileSystemsManager> _fileSystemsManagerMock = new Mock<IFileSystemsManager>();
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
        private readonly Mock<ILogger<DocumentsAdderService>> _loggerMock = new Mock<ILogger<DocumentsAdderService>>();
        private readonly Mock<ICategoriesGetterService> _categoriesGetterServiceMock = new Mock<ICategoriesGetterService>();
        private readonly Mock<IDocumentsAdderRepository> _documentsAdderRepositoryMock = new Mock<IDocumentsAdderRepository>();
        private readonly Mock<IStatusesGetterRepository> _statusesGetterRepositoryMock = new Mock<IStatusesGetterRepository>();
        private readonly Mock<IDocumentTypesGetterRepository> _documentTypesGetterRepositoryMock = new Mock<IDocumentTypesGetterRepository>();

        private readonly DocumentsAdderService _service;

        public DocumentsAdderServiceTest()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _service = new DocumentsAdderService(
                _categoriesGetterServiceMock.Object,
                _documentsAdderRepositoryMock.Object,
                _statusesGetterRepositoryMock.Object,
                _documentTypesGetterRepositoryMock.Object,
                _fileSystemsManagerMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateDocument_ShouldThrowArgumentException_WhenFileIsNull()
        {
            // Arrange
            var request = _fixture.Build<DocumentRequestDTO>()
                .With(x => x.File, (IFormFile?)null)
                .Create();

            // Act
            Func<Task> act = async () => await _service.CreateDocument(request);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Archivo inválido.");
        }

        [Fact]
        public async Task CreateDocument_ShouldThrowArgumentException_WhenStatusIdIsEmpty()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(100);

            var request = _fixture.Build<DocumentRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .With(x => x.StatusId, Guid.Empty)
                .Create();

            _fileSystemsManagerMock.Setup(x =>
                    x.SaveAsync(It.IsAny<IFormFile>(), "documentos", It.IsAny<string>()))
                .ReturnsAsync("documentos/test.pdf");

            // Act
            Func<Task> act = async () => await _service.CreateDocument(request);

            // Assert
            await act.Should()
                .ThrowAsync<CreateObjectException>()
                .WithMessage("Error al crear el documento.");

            _fileSystemsManagerMock.Verify(x =>
                x.DeleteAsync("test.pdf", "documentos"),
                Times.Once);
        }

        [Fact]
        public async Task CreateDocument_ShouldThrowNotFoundException_WhenStatusDoesNotExist()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(100);

            var request = _fixture.Build<DocumentRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .Create();

            _fileSystemsManagerMock.Setup(x =>
                    x.SaveAsync(It.IsAny<IFormFile>(), "documentos", It.IsAny<string>()))
                .ReturnsAsync("documentos/test.pdf");

            _statusesGetterRepositoryMock.Setup(x =>
                    x.GetStatus(request.StatusId))!
                .ReturnsAsync((StatusResponseDTO?)null);

            // Act
            Func<Task> act = async () => await _service.CreateDocument(request);

            // Assert
            await act.Should()
                .ThrowAsync<CreateObjectException>();

            _fileSystemsManagerMock.Verify(x =>
                x.DeleteAsync("test.pdf", "documentos"),
                Times.Once);
        }

        [Fact]
        public async Task CreateDocument_ShouldThrowNotFoundException_WhenDocumentTypeDoesNotExist()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(100);

            var request = _fixture.Build<DocumentRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .Create();

            var status = _fixture.Build<Status>()
                .With(x => x.Name, "Publicado")
                .Create();

            _fileSystemsManagerMock.Setup(x =>
                    x.SaveAsync(It.IsAny<IFormFile>(), "documentos", It.IsAny<string>()))
                .ReturnsAsync("documentos/test.xyz");

            _statusesGetterRepositoryMock.Setup(x =>
                    x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock.Setup(x =>
                    x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _documentTypesGetterRepositoryMock.Setup(x =>
                    x.GetDocumentTypeIdByExtension(".xyz"))
                .Returns((Guid?)null);

            // Act
            Func<Task> act = async () => await _service.CreateDocument(request);

            // Assert
            await act.Should()
                .ThrowAsync<CreateObjectException>();

            _fileSystemsManagerMock.Verify(x =>
                x.DeleteAsync("test.xyz", "documentos"),
                Times.Once);
        }

        [Fact]
        public async Task CreateDocument_ShouldCreateDocumentSuccessfully()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(500);

            var request = _fixture.Build<DocumentRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .Create();

            var status = _fixture.Build<Status>()
                .With(x => x.Name, "Borrador")
                .Create();

            var documentTypeId = Guid.NewGuid();

            var createdDocument = _fixture.Build<AsadaLisboaBackend.Models.Document>()
                .With(x => x.Title, request.Title)
                .Create();

            _fileSystemsManagerMock.Setup(x =>
                    x.SaveAsync(It.IsAny<IFormFile>(), "documentos", It.IsAny<string>()))
                .ReturnsAsync("documentos/test.pdf");

            _statusesGetterRepositoryMock.Setup(x =>
                    x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock.Setup(x =>
                    x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _documentTypesGetterRepositoryMock.Setup(x =>
                    x.GetDocumentTypeIdByExtension(".pdf"))
                .Returns(documentTypeId);

            _documentsAdderRepositoryMock.Setup(x =>
                    x.CreateDocument(It.IsAny<AsadaLisboaBackend.Models.Document>()))
                .ReturnsAsync(createdDocument);

            // Act
            var result = await _service.CreateDocument(request);

            // Assert
            result.Should().NotBeNull();

            _documentsAdderRepositoryMock.Verify(x =>
                x.CreateDocument(It.IsAny<AsadaLisboaBackend.Models.Document>()),
                Times.Once);

            _memoryCachesServiceMock.Verify(x =>
                x.ChangeVersion(Constants.CACHE_DOCUMENTS),
                Times.Once);
        }

        [Fact]
        public async Task CreateDocument_ShouldDeleteFile_WhenRepositoryFails()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(100);

            var request = _fixture.Build<DocumentRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .Create();

            var status = _fixture.Build<Status>()
                .With(x => x.Name, "Publicado")
                .Create();

            _fileSystemsManagerMock.Setup(x =>
                    x.SaveAsync(It.IsAny<IFormFile>(), "documentos", It.IsAny<string>()))
                .ReturnsAsync("documentos/test.pdf");

            _statusesGetterRepositoryMock.Setup(x =>
                    x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock.Setup(x =>
                    x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _documentTypesGetterRepositoryMock.Setup(x =>
                    x.GetDocumentTypeIdByExtension(".pdf"))
                .Returns(Guid.NewGuid());

            _documentsAdderRepositoryMock.Setup(x =>
                    x.CreateDocument(It.IsAny<AsadaLisboaBackend.Models.Document>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await _service.CreateDocument(request);

            // Assert
            await act.Should()
                .ThrowAsync<CreateObjectException>();

            _fileSystemsManagerMock.Verify(x =>
                x.DeleteAsync("test.pdf", "documentos"),
                Times.Once);
        }
    }
}