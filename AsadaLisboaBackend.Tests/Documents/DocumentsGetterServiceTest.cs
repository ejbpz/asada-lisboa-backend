using Moq;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Documents;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Services.Documents
{
    public class DocumentsGetterServiceTest
    {
        private readonly Mock<IMemoryCachesService> _memoryCacheMock = new();
        private readonly Mock<IDocumentsGetterRepository> _repositoryMock = new();

        private readonly DocumentsGetterService _service;

        public DocumentsGetterServiceTest()
        {
            _service = new DocumentsGetterService(
                _repositoryMock.Object,
                _memoryCacheMock.Object
            );
        }

        [Fact]
        public async Task GetDocuments_ShouldReturnPagedDocuments()
        {
            // Arrange
            var request = new SearchSortRequestDTO();

            var expected = new PageResponseDTO<DocumentMinimalResponseDTO>
            {
                Data = new List<DocumentMinimalResponseDTO>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Documento 1"
                }
            }
            };

            _memoryCacheMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<DocumentMinimalResponseDTO>>(
                    Constants.CACHE_DOCUMENTS,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<DocumentMinimalResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetDocuments(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);

            _memoryCacheMock.Verify(
                x => x.GetOrCreateCacheList<PageResponseDTO<DocumentMinimalResponseDTO>>(
                    Constants.CACHE_DOCUMENTS,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<DocumentMinimalResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Fact]
        public async Task GetDocument_ShouldReturnDocumentResponseDTO()
        {
            // Arrange
            var id = Guid.NewGuid();

            var document = new Document
            {
                Id = id,
                Title = "Documento Test",
                Description = "Descripción",
                Slug = "documento-test",
                Url = "https://localhost/document.pdf",
                FileName = "document.pdf",
                FilePath = "documentos/document.pdf",
                PublicationDate = DateTime.UtcNow
            };

            _memoryCacheMock
                .Setup(x => x.GetOrCreateCache<Document>(
                    $"{Constants.CACHE_DOCUMENTS}_{id}",
                    It.IsAny<Func<Task<Document>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(document);

            // Act
            var result = await _service.GetDocument(id);

            // Assert
            result.Should().NotBeNull();

            result.Id.Should().Be(document.Id);
            result.Title.Should().Be(document.Title);
            result.Description.Should().Be(document.Description);
            result.Slug.Should().Be(document.Slug);

            _memoryCacheMock.Verify(
                x => x.GetOrCreateCache<Document>(
                    $"{Constants.CACHE_DOCUMENTS}_{id}",
                    It.IsAny<Func<Task<Document>>>(),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Fact]
        public async Task GetDocumentBySlug_ShouldReturnDocumentResponseDTO()
        {
            // Arrange
            var slug = "mi-documento";

            var document = new Document
            {
                Id = Guid.NewGuid(),
                Title = "Documento por Slug",
                Description = "Descripción",
                Slug = slug,
                Url = "https://localhost/document.pdf",
                FileName = "document.pdf",
                FilePath = "documentos/document.pdf",
                PublicationDate = DateTime.UtcNow
            };

            _memoryCacheMock
                .Setup(x => x.GetOrCreateCache<Document>(
                    $"{Constants.CACHE_DOCUMENTS}_{slug}",
                    It.IsAny<Func<Task<Document>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(document);

            // Act
            var result = await _service.GetDocumentBySlug(slug);

            // Assert
            result.Should().NotBeNull();

            result.Id.Should().Be(document.Id);
            result.Title.Should().Be(document.Title);
            result.Slug.Should().Be(document.Slug);

            _memoryCacheMock.Verify(
                x => x.GetOrCreateCache<Document>(
                    $"{Constants.CACHE_DOCUMENTS}_{slug}",
                    It.IsAny<Func<Task<Document>>>(),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Fact]
        public async Task GetDocument_ShouldCallRepositoryInsideCacheFactory()
        {
            // Arrange
            var id = Guid.NewGuid();

            var document = new Document
            {
                Id = id,
                Title = "Documento"
            };

            Func<Task<Document>>? factory = null;

            _memoryCacheMock
                .Setup(x => x.GetOrCreateCache<Document>(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<Document>>>(),
                    It.IsAny<TimeSpan>()))
                .Callback<string, Func<Task<Document>>, TimeSpan>((_, f, _) =>
                {
                    factory = f;
                })
                .ReturnsAsync(document);

            _repositoryMock
                .Setup(x => x.GetDocument(id))
                .ReturnsAsync(document);

            // Act
            await _service.GetDocument(id);

            // Ejecutar manualmente el factory
            await factory!();

            // Assert
            _repositoryMock.Verify(
                x => x.GetDocument(id),
                Times.Once);
        }

        [Fact]
        public async Task GetDocumentBySlug_ShouldCallRepositoryInsideCacheFactory()
        {
            // Arrange
            var slug = "slug-test";

            var document = new Document
            {
                Id = Guid.NewGuid(),
                Title = "Documento"
            };

            Func<Task<Document>>? factory = null;

            _memoryCacheMock
                .Setup(x => x.GetOrCreateCache<Document>(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<Document>>>(),
                    It.IsAny<TimeSpan>()))
                .Callback<string, Func<Task<Document>>, TimeSpan>((_, f, _) =>
                {
                    factory = f;
                })
                .ReturnsAsync(document);

            _repositoryMock
                .Setup(x => x.GetDocumentBySlug(slug))
                .ReturnsAsync(document);

            // Act
            await _service.GetDocumentBySlug(slug);

            // Ejecutar factory manualmente
            await factory!();

            // Assert
            _repositoryMock.Verify(
                x => x.GetDocumentBySlug(slug),
                Times.Once);
        }
    }
}