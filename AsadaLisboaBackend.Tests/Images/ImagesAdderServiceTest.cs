using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using Elastic.Clients.Elasticsearch;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Images;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Services.Images
{
    public class ImagesAdderServiceTests
    {
        private readonly Mock<IFileSystemsManager> _fileSystemsMock = new Mock<IFileSystemsManager>();
        private readonly Mock<IMemoryCachesService> _memoryCacheMock = new Mock<IMemoryCachesService>();
        private readonly Mock<ILogger<ImagesAdderService>> _loggerMock = new Mock<ILogger<ImagesAdderService>>();
        private readonly Mock<IImagesAdderRepository> _imagesRepositoryMock = new Mock<IImagesAdderRepository>();
        private readonly Mock<ICategoriesGetterService> _categoriesServiceMock = new Mock<ICategoriesGetterService>();
        private readonly Mock<IStatusesGetterRepository> _statusRepositoryMock = new Mock<IStatusesGetterRepository>();

        private readonly Mock<ElasticsearchClient> _elasticMock;

        private readonly ImagesAdderService _service;

        public ImagesAdderServiceTests()
        {
            _elasticMock = new Mock<ElasticsearchClient>();

            _service = new ImagesAdderService(
                _imagesRepositoryMock.Object,
                _fileSystemsMock.Object,
                _categoriesServiceMock.Object,
                _statusRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCacheMock.Object,
                _elasticMock.Object
            );
        }

        [Fact]
        public async Task CreateImage_ShouldThrowArgumentException_WhenFileIsNull()
        {
            // Arrange
            var request = new ImageRequestDTO
            {
                File = null
            };

            // Act
            Func<Task> action = async () => await _service.CreateImage(request);

            // Assert
            await action.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Archivo inválido.");
        }

        [Fact]
        public async Task CreateImage_ShouldThrowArgumentException_WhenFileIsEmpty()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock
                .Setup(x => x.Length)
                .Returns(0);

            var request = new ImageRequestDTO
            {
                File = fileMock.Object
            };

            // Act
            Func<Task> action = async () => await _service.CreateImage(request);

            // Assert
            await action.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Archivo inválido.");
        }

        [Fact]
        public async Task CreateImage_ShouldThrowCreateObjectException_WhenStatusIdIsEmpty()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock
                .Setup(x => x.Length)
                .Returns(100);

            _fileSystemsMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<IFormFile>(),
                    "imagenes",
                    It.IsAny<string>()))
                .ReturnsAsync("imagenes/test.jpg");

            var request = new ImageRequestDTO
            {
                Title = "Imagen Test",
                StatusId = Guid.Empty,
                File = fileMock.Object,
                Categories = []
            };

            // Act
            Func<Task> action = async () => await _service.CreateImage(request);

            // Assert
            await action.Should()
                .ThrowAsync<CreateObjectException>();

            _fileSystemsMock.Verify(
                x => x.DeleteAsync("test.jpg", "imagenes"),
                Times.Once);
        }

        [Fact]
        public async Task CreateImage_ShouldThrowCreateObjectException_WhenStatusDoesNotExist()
        {
            // Arrange
            var statusId = Guid.NewGuid();

            var fileMock = new Mock<IFormFile>();

            fileMock
                .Setup(x => x.Length)
                .Returns(100);

            _fileSystemsMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<IFormFile>(),
                    "imagenes",
                    It.IsAny<string>()))
                .ReturnsAsync("imagenes/test.jpg");

            _statusRepositoryMock
                .Setup(x => x.GetStatus(statusId))!
                .ReturnsAsync((StatusResponseDTO?)null);

            var request = new ImageRequestDTO
            {
                Title = "Imagen Test",
                StatusId = statusId,
                File = fileMock.Object,
                Categories = []
            };

            // Act
            Func<Task> action = async () => await _service.CreateImage(request);

            // Assert
            await action.Should()
                .ThrowAsync<CreateObjectException>();

            _fileSystemsMock.Verify(
                x => x.DeleteAsync("test.jpg", "imagenes"),
                Times.Once);
        }

        [Fact]
        public async Task CreateImage_ShouldCreateImageSuccessfully()
        {
            // Arrange
            var statusId = Guid.NewGuid();

            var fileMock = new Mock<IFormFile>();

            fileMock
                .Setup(x => x.Length)
                .Returns(500);

            var status = new Status
            {
                Id = statusId,
                Name = "Borrador"
            };

            var categories = new HashSet<Category>()
        {
            new()
            {
                Id = Guid.NewGuid()
            }
        };

            var imageCreated = new Image
            {
                Id = Guid.NewGuid(),
                Title = "Imagen Test",
                Description = "Descripción",
                Url = "imagenes/test.jpg",
                FileName = "test.jpg",
                FilePath = "imagenes/test.jpg",
                FileSize = 500,
                PublicationDate = DateTime.UtcNow
            };

            _fileSystemsMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<IFormFile>(),
                    "imagenes",
                    It.IsAny<string>()))
                .ReturnsAsync("imagenes/test.jpg");

            _statusRepositoryMock
                .Setup(x => x.GetStatus(statusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesServiceMock
                .Setup(x => x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(categories);

            _imagesRepositoryMock
                .Setup(x => x.CreateImage(It.IsAny<Image>()))
                .ReturnsAsync(imageCreated);

            var request = new ImageRequestDTO
            {
                Title = "Imagen Test",
                Description = "Descripción",
                StatusId = statusId,
                File = fileMock.Object,
                Categories = []
            };

            // Act
            var result = await _service.CreateImage(request);

            // Assert
            result.Should().NotBeNull();

            result.Id.Should().Be(imageCreated.Id);
            result.Title.Should().Be(imageCreated.Title);

            _imagesRepositoryMock.Verify(
                x => x.CreateImage(It.IsAny<Image>()),
                Times.Once);

            _memoryCacheMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_IMAGES),
                Times.Once);
        }

        [Fact]
        public async Task CreateImage_ShouldDeleteFile_WhenRepositoryThrowsException()
        {
            // Arrange
            var statusId = Guid.NewGuid();

            var fileMock = new Mock<IFormFile>();

            fileMock
                .Setup(x => x.Length)
                .Returns(500);

            var status = new Status
            {
                Id = statusId,
                Name = "Borrador"
            };

            _fileSystemsMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<IFormFile>(),
                    "imagenes",
                    It.IsAny<string>()))
                .ReturnsAsync("imagenes/test.jpg");

            _statusRepositoryMock
                .Setup(x => x.GetStatus(statusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesServiceMock
                .Setup(x => x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _imagesRepositoryMock
                .Setup(x => x.CreateImage(It.IsAny<Image>()))
                .ThrowsAsync(new Exception());

            var request = new ImageRequestDTO
            {
                Title = "Imagen Test",
                Description = "Descripción",
                StatusId = statusId,
                File = fileMock.Object,
                Categories = []
            };

            // Act
            Func<Task> action = async () => await _service.CreateImage(request);

            // Assert
            await action.Should()
                .ThrowAsync<CreateObjectException>();

            _fileSystemsMock.Verify(
                x => x.DeleteAsync("test.jpg", "imagenes"),
                Times.Once);
        }
    }
}