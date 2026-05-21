using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Images;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Services.Images
{
    public class ImagesUpdaterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IFileSystemsManager> _fileSystemsManagerMock = new Mock<IFileSystemsManager>();
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
        private readonly Mock<ILogger<ImagesUpdaterService>> _loggerMock = new Mock<ILogger<ImagesUpdaterService>>();
        private readonly Mock<IImagesGetterRepository> _imagesGetterRepositoryMock = new Mock<IImagesGetterRepository>();
        private readonly Mock<ICategoriesGetterService> _categoriesGetterServiceMock = new Mock<ICategoriesGetterService>();
        private readonly Mock<IImagesUpdaterRepository> _imagesUpdaterRepositoryMock = new Mock<IImagesUpdaterRepository>();
        private readonly Mock<IStatusesGetterRepository> _statusesGetterRepositoryMock = new Mock<IStatusesGetterRepository>();

        private readonly ImagesUpdaterService _service;

        public ImagesUpdaterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _service = new ImagesUpdaterService(
                applicationDbContext: null!,
                fileSystems: _fileSystemsManagerMock.Object,
                imagesUpdaterRepository: _imagesUpdaterRepositoryMock.Object,
                imagesGetterRespository: _imagesGetterRepositoryMock.Object,
                categoriesGetterService: _categoriesGetterServiceMock.Object,
                statusesGetterRepository: _statusesGetterRepositoryMock.Object,
                logger: _loggerMock.Object,
                memoryCachesService: _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task UpdateImage_ShouldThrowNotFoundException_WhenImageDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Build<ImageUpdateRequestDTO>()
                .Without(x => x.File)
                .Create();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))!
                .ReturnsAsync((Image?)null);

            // Act
            Func<Task> act = async () =>
                await _service.UpdateImage(id, request);

            // Assert
            await act
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Imagen no encontrada.");
        }

        [Fact]
        public async Task UpdateImage_ShouldThrowArgumentException_WhenStatusIdIsEmpty()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Create<Image>();

            var request = _fixture.Build<ImageUpdateRequestDTO>()
                .With(x => x.StatusId, Guid.Empty)
                .Without(x => x.File)
                .Create();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))
                .ReturnsAsync(image);

            // Act
            Func<Task> act = async () =>
                await _service.UpdateImage(id, request);

            // Assert
            await act
                .Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("StatusId inválido.");
        }

        [Fact]
        public async Task UpdateImage_ShouldThrowNotFoundException_WhenStatusDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Create<Image>();

            var request = _fixture.Build<ImageUpdateRequestDTO>()
                .Without(x => x.File)
                .Create();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))
                .ReturnsAsync(image);

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))!
                .ReturnsAsync((StatusResponseDTO?)null);

            // Act
            Func<Task> act = async () =>
                await _service.UpdateImage(id, request);

            // Assert
            await act
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Status no encontrado.");
        }

        [Fact]
        public async Task UpdateImage_ShouldUpdateWithoutFile_WhenFileIsNull()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Build<Image>()
                .With(x => x.Id, id)
                .Create();

            var status = _fixture.Build<Status>()
                .With(x => x.Name, "Borrador")
                .Create();

            var categories = _fixture.Create<HashSet<Category>>();

            var request = _fixture.Build<ImageUpdateRequestDTO>()
                .With(x => x.File, (IFormFile?)null)
                .Create();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))
                .ReturnsAsync(image);

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock
                .Setup(x => x.ToCreateCategories(request.Categories))
                .ReturnsAsync(categories);

            _imagesUpdaterRepositoryMock
                .Setup(x => x.UpdateImage(It.IsAny<Image>()))
                .ReturnsAsync(image);

            // Act
            var result = await _service.UpdateImage(id, request);

            // Assert
            result.Should().NotBeNull();

            _imagesUpdaterRepositoryMock.Verify(
                x => x.UpdateImage(It.IsAny<Image>()),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(Constants.CACHE_IMAGES, image.Id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_IMAGES),
                Times.Once);
        }

        [Fact]
        public async Task UpdateImage_ShouldUpdateWithFile_WhenFileIsValid()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Build<Image>()
                .With(x => x.Id, id)
                .With(x => x.FilePath, string.Empty)
                .Create();

            var status = _fixture.Build<Status>()
                .With(x => x.Name, "Borrador")
                .Create();

            var categories = _fixture.Create<HashSet<Category>>();

            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(100);

            fileMock.Setup(x => x.FileName)
                .Returns("image.jpg");

            var request = _fixture.Build<ImageUpdateRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .Create();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))
                .ReturnsAsync(image);

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock
                .Setup(x => x.ToCreateCategories(request.Categories))
                .ReturnsAsync(categories);

            _fileSystemsManagerMock
                .Setup(x => x.SaveAsync(
                    request.File!,
                    "imagenes",
                    It.IsAny<string>()))
                .ReturnsAsync("imagenes/test-image.jpg");

            _imagesUpdaterRepositoryMock
                .Setup(x => x.UpdateImage(It.IsAny<Image>()))
                .ReturnsAsync(image);

            // Act
            var result = await _service.UpdateImage(id, request);

            // Assert
            result.Should().NotBeNull();

            image.Url.Should().Be("imagenes/test-image.jpg");
            image.FileName.Should().Be("test-image.jpg");
            image.FilePath.Should().Be("imagenes/test-image.jpg");
            image.FileSize.Should().Be(100);
        }

        [Fact]
        public async Task UpdateImage_ShouldThrowCreateObjectException_WhenSaveFileFails()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Create<Image>();

            var status = _fixture.Build<Status>()
                .With(x => x.Name, "Borrador")
                .Create();

            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(100);

            var request = _fixture.Build<ImageUpdateRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .Create();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))
                .ReturnsAsync(image);

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock
                .Setup(x => x.ToCreateCategories(request.Categories))
                .ReturnsAsync(new HashSet<Category>());

            _fileSystemsManagerMock
                .Setup(x => x.SaveAsync(
                    request.File!,
                    "imagenes",
                    It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () =>
                await _service.UpdateImage(id, request);

            // Assert
            await act
                .Should()
                .ThrowAsync<CreateObjectException>()
                .WithMessage("Error al actualizar la imagen.");
        }
    }
}