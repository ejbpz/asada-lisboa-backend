using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Services.Documents;
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
    public class DocumentsUpdaterServiceTests
    {
        private readonly Fixture _fixture = new();

        private readonly Mock<IFileSystemsManager> _fileSystemsMock = new Mock<IFileSystemsManager>();
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
        private readonly Mock<ILogger<DocumentsUpdaterService>> _loggerMock = new Mock<ILogger<DocumentsUpdaterService>>();
        private readonly Mock<ICategoriesGetterService> _categoriesGetterServiceMock = new Mock<ICategoriesGetterService>();
        private readonly Mock<IStatusesGetterRepository> _statusesGetterRepositoryMock = new Mock<IStatusesGetterRepository>();
        private readonly Mock<IDocumentsGetterRepository> _documentsGetterRepositoryMock = new Mock<IDocumentsGetterRepository>();
        private readonly Mock<IDocumentsUpdaterRepository> _documentsUpdaterRepositoryMock = new Mock<IDocumentsUpdaterRepository>();
        private readonly Mock<IDocumentTypesGetterRepository> _documentTypesGetterRepositoryMock = new Mock<IDocumentTypesGetterRepository>();

        private readonly DocumentsUpdaterService _service;

        public DocumentsUpdaterServiceTests()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _service = new DocumentsUpdaterService(
                _fileSystemsMock.Object,
                _documentsGetterRepositoryMock.Object,
                _documentsUpdaterRepositoryMock.Object,
                _categoriesGetterServiceMock.Object,
                _documentTypesGetterRepositoryMock.Object,
                _statusesGetterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task UpdateDocument_ShouldThrowNotFoundException_WhenDocumentDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Build<DocumentUpdateRequestDTO>()
                .Without(x => x.File)
                .Create();

            _documentsGetterRepositoryMock.Setup(x =>
                    x.GetDocument(id))!
                .ReturnsAsync((Document?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateDocument(id, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Documento no encontrado.");
        }

        [Fact]
        public async Task UpdateDocument_ShouldThrowArgumentException_WhenStatusIdIsEmpty()
        {
            // Arrange
            var document = _fixture.Create<Document>();

            var request = _fixture.Build<DocumentUpdateRequestDTO>()
                .With(x => x.StatusId, Guid.Empty)
                .Without(x => x.File)
                .Create();

            _documentsGetterRepositoryMock.Setup(x =>
                    x.GetDocument(document.Id))
                .ReturnsAsync(document);

            // Act
            Func<Task> act = async () => await _service.UpdateDocument(document.Id, request);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("StatusId inválido.");
        }

        [Fact]
        public async Task UpdateDocument_ShouldThrowNotFoundException_WhenStatusDoesNotExist()
        {
            // Arrange
            var document = _fixture.Create<Document>();

            var request = _fixture.Build<DocumentUpdateRequestDTO>()
                .Without(x => x.File)
                .Create();

            _documentsGetterRepositoryMock.Setup(x =>
                    x.GetDocument(document.Id))
                .ReturnsAsync(document);

            _statusesGetterRepositoryMock.Setup(x =>
                    x.GetStatus(request.StatusId))!
                .ReturnsAsync((StatusResponseDTO?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateDocument(document.Id, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Status no encontrado.");
        }

        [Fact]
        public async Task UpdateDocument_ShouldUpdateSuccessfully_WithoutFile()
        {
            // Arrange
            var document = _fixture.Create<Document>();

            var status = _fixture.Build<Status>()
                .With(x => x.Name, "Borrador")
                .Create();

            var request = _fixture.Build<DocumentUpdateRequestDTO>()
                .With(x => x.File, (IFormFile?)null)
                .Create();

            _documentsGetterRepositoryMock.Setup(x =>
                    x.GetDocument(document.Id))
                .ReturnsAsync(document);

            _statusesGetterRepositoryMock.Setup(x =>
                    x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock.Setup(x =>
                    x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _documentsUpdaterRepositoryMock.Setup(x =>
                    x.UpdateDocument(It.IsAny<Document>()))
                .ReturnsAsync(document);

            // Act
            var result = await _service.UpdateDocument(document.Id, request);

            // Assert
            result.Should().NotBeNull();

            _documentsUpdaterRepositoryMock.Verify(x =>
                x.UpdateDocument(It.IsAny<Document>()),
                Times.Once);

            _memoryCachesServiceMock.Verify(x =>
                x.RemoveById(Constants.CACHE_DOCUMENTS, document.Id),
                Times.Once);

            _memoryCachesServiceMock.Verify(x =>
                x.ChangeVersion(Constants.CACHE_DOCUMENTS),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDocument_ShouldUpdateSuccessfully_WithFile()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(500);

            var document = _fixture.Build<Document>()
                .With(x => x.FilePath, "documentos/old.pdf")
                .With(x => x.FileName, "old.pdf")
                .Create();

            var status = _fixture.Build<Status>()
                .With(x => x.Name, "Borrador")
                .Create();

            var request = _fixture.Build<DocumentUpdateRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .Create();

            _documentsGetterRepositoryMock.Setup(x =>
                    x.GetDocument(document.Id))
                .ReturnsAsync(document);

            _statusesGetterRepositoryMock.Setup(x =>
                    x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock.Setup(x =>
                    x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _fileSystemsMock.Setup(x =>
                    x.SaveAsync(fileMock.Object, "documentos", It.IsAny<string>()))
                .ReturnsAsync("documentos/new.pdf");

            _documentTypesGetterRepositoryMock.Setup(x =>
                    x.GetDocumentTypeIdByExtension(".pdf"))
                .Returns(Guid.NewGuid());

            _documentsUpdaterRepositoryMock.Setup(x =>
                    x.UpdateDocument(It.IsAny<Document>()))
                .ReturnsAsync(document);

            // Act
            var result = await _service.UpdateDocument(document.Id, request);

            // Assert
            result.Should().NotBeNull();

            document.Url.Should().Be("documentos/new.pdf");
            document.FileName.Should().Be("new.pdf");

            _documentsUpdaterRepositoryMock.Verify(x =>
                x.UpdateDocument(It.IsAny<Document>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDocument_ShouldThrowCreateObjectException_WhenSaveFileFails()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(200);

            var document = _fixture.Create<Document>();

            var status = _fixture.Create<Status>();

            var request = _fixture.Build<DocumentUpdateRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .Create();

            _documentsGetterRepositoryMock.Setup(x =>
                    x.GetDocument(document.Id))
                .ReturnsAsync(document);

            _statusesGetterRepositoryMock.Setup(x =>
                    x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock.Setup(x =>
                    x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _fileSystemsMock.Setup(x =>
                    x.SaveAsync(It.IsAny<IFormFile>(), "documentos", It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await _service.UpdateDocument(document.Id, request);

            // Assert
            await act.Should()
                .ThrowAsync<CreateObjectException>()
                .WithMessage("Error al actualizar el documento.");
        }

        [Fact]
        public async Task UpdateDocument_ShouldThrowNotFoundException_WhenDocumentTypeIsInvalid()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(500);

            var document = _fixture.Create<Document>();

            var status = _fixture.Create<Status>();

            var request = _fixture.Build<DocumentUpdateRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .Create();

            _documentsGetterRepositoryMock.Setup(x =>
                    x.GetDocument(document.Id))
                .ReturnsAsync(document);

            _statusesGetterRepositoryMock.Setup(x =>
                    x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock.Setup(x =>
                    x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _fileSystemsMock.Setup(x =>
                    x.SaveAsync(It.IsAny<IFormFile>(), "documentos", It.IsAny<string>()))
                .ReturnsAsync("documentos/file.xyz");

            _documentTypesGetterRepositoryMock.Setup(x =>
                    x.GetDocumentTypeIdByExtension(".xyz"))
                .Returns((Guid?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateDocument(document.Id, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Tipo de documento no soportado.");
        }

        [Fact]
        public async Task UpdateDocument_ShouldIndexElastic_WhenStatusIsPublicado()
        {
            // Arrange
            var document = _fixture.Create<Document>();

            var status = _fixture.Build<Status>()
                .With(x => x.Name, "Publicado")
                .Create();

            var request = _fixture.Build<DocumentUpdateRequestDTO>()
                .With(x => x.File, (IFormFile?)null)
                .Create();

            _documentsGetterRepositoryMock.Setup(x =>
                    x.GetDocument(document.Id))
                .ReturnsAsync(document);

            _statusesGetterRepositoryMock.Setup(x =>
                    x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock.Setup(x =>
                    x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _documentsUpdaterRepositoryMock.Setup(x =>
                    x.UpdateDocument(It.IsAny<Document>()))
                .ReturnsAsync(document);

            // Act
            await _service.UpdateDocument(document.Id, request);

            // Assert
            _documentsUpdaterRepositoryMock.Verify(x =>
                x.UpdateDocument(It.IsAny<Document>()),
                Times.Once);
        }
    }
}