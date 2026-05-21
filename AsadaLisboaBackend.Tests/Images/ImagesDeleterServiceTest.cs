using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Images;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Services.Images
{
    public class ImagesDeleterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IFileSystemsManager> _fileSystemsManagerMock = new Mock<IFileSystemsManager>();
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
        private readonly Mock<ILogger<ImagesDeleterService>> _loggerMock = new Mock<ILogger<ImagesDeleterService>>();
        private readonly Mock<IImagesGetterRepository> _imagesGetterRepositoryMock = new Mock<IImagesGetterRepository>();
        private readonly Mock<IImagesDeleterRepository> _imagesDeleterRepositoryMock = new Mock<IImagesDeleterRepository>();

        private readonly ImagesDeleterService _service;

        public ImagesDeleterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _service = new ImagesDeleterService(
                fileSystems: _fileSystemsManagerMock.Object,
                imagesDeleterRepository: _imagesDeleterRepositoryMock.Object,
                imagesGetterRepository: _imagesGetterRepositoryMock.Object,
                logger: _loggerMock.Object,
                memoryCachesService: _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task DeleteImage_ShouldThrowNotFoundException_WhenImageDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))!
                .ReturnsAsync((Image?)null);

            // Act
            Func<Task> act = async () =>
                await _service.DeleteImage(id);

            // Assert
            await act
                .Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Imagen no encontrada.");

            _imagesDeleterRepositoryMock.Verify(
                x => x.DeleteImage(It.IsAny<Guid>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteImage_ShouldDeleteFile_WhenFileNameExists()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Build<Image>()
                .With(x => x.Id, id)
                .With(x => x.FileName, "image.jpg")
                .Create();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))
                .ReturnsAsync(image);

            _imagesDeleterRepositoryMock
                .Setup(x => x.DeleteImage(id))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteImage(id);

            // Assert
            _fileSystemsManagerMock.Verify(
                x => x.DeleteAsync("image.jpg", "imagenes"),
                Times.Once);

            _imagesDeleterRepositoryMock.Verify(
                x => x.DeleteImage(id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(Constants.CACHE_IMAGES, image.Id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_IMAGES),
                Times.Once);
        }

        [Fact]
        public async Task DeleteImage_ShouldNotDeleteFile_WhenFileNameIsNull()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Build<Image>()
                .With(x => x.Id, id)
                .With(x => x.FileName, (string?)null)
                .Create();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))
                .ReturnsAsync(image);

            _imagesDeleterRepositoryMock
                .Setup(x => x.DeleteImage(id))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteImage(id);

            // Assert
            _fileSystemsManagerMock.Verify(
                x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);

            _imagesDeleterRepositoryMock.Verify(
                x => x.DeleteImage(id),
                Times.Once);
        }

        [Fact]
        public async Task DeleteImage_ShouldNotDeleteFile_WhenFileNameIsWhitespace()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Build<Image>()
                .With(x => x.Id, id)
                .With(x => x.FileName, " ")
                .Create();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))
                .ReturnsAsync(image);

            _imagesDeleterRepositoryMock
                .Setup(x => x.DeleteImage(id))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteImage(id);

            // Assert
            _fileSystemsManagerMock.Verify(
                x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteImage_ShouldCallRepositoryAndCacheMethods()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Build<Image>()
                .With(x => x.Id, id)
                .With(x => x.FileName, "image.jpg")
                .Create();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))
                .ReturnsAsync(image);

            _imagesDeleterRepositoryMock
                .Setup(x => x.DeleteImage(id))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteImage(id);

            // Assert
            _imagesDeleterRepositoryMock.Verify(
                x => x.DeleteImage(id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(Constants.CACHE_IMAGES, image.Id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_IMAGES),
                Times.Once);
        }

        [Fact]
        public async Task DeleteImage_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Build<Image>()
                .With(x => x.Id, id)
                .With(x => x.FileName, "image.jpg")
                .Create();

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))
                .ReturnsAsync(image);

            _imagesDeleterRepositoryMock
                .Setup(x => x.DeleteImage(id))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () =>
                await _service.DeleteImage(id);

            // Assert
            await act
                .Should()
                .ThrowAsync<Exception>();

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(It.IsAny<string>()),
                Times.Never);
        }
    }
}