using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.News;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.News
{
    public class NewsAdderServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IFileSystemsManager> _fileSystemsMock;
        private readonly Mock<ILogger<NewsAdderService>> _loggerMock;
        private readonly Mock<INewsAdderRepository> _newsAdderRepositoryMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<IEditorsUpdaterService> _editorsUpdaterServiceMock;
        private readonly Mock<ICategoriesGetterService> _categoriesGetterServiceMock;
        private readonly Mock<IStatusesGetterRepository> _statusesGetterRepositoryMock;

        private readonly NewsAdderService _service;

        public NewsAdderServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fileSystemsMock = new Mock<IFileSystemsManager>();
            _loggerMock = new Mock<ILogger<NewsAdderService>>();
            _newsAdderRepositoryMock = new Mock<INewsAdderRepository>();
            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
            _editorsUpdaterServiceMock = new Mock<IEditorsUpdaterService>();
            _categoriesGetterServiceMock = new Mock<ICategoriesGetterService>();
            _statusesGetterRepositoryMock = new Mock<IStatusesGetterRepository>();

            _service = new NewsAdderService(
                _newsAdderRepositoryMock.Object,
                _editorsUpdaterServiceMock.Object,
                _statusesGetterRepositoryMock.Object,
                _categoriesGetterServiceMock.Object,
                _fileSystemsMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateNew_Should_Create_New_Successfully()
        {
            // Arrange
            var request = _fixture.Build<NewRequestDTO>()
                .With(x => x.StatusId, Guid.NewGuid())
                .Without(x => x.File)
                .Create();

            var status = _fixture.Build<Status>()
                .With(x => x.Id, request.StatusId)
                .Create();

            var categories = _fixture.CreateMany<Category>(3).ToList();

            var createdNew = _fixture.Build<New>()
                .With(x => x.Id, Guid.NewGuid())
                .Create();

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock
                .Setup(x => x.ToCreateCategories(request.Categories))
                .ReturnsAsync(categories.ToHashSet());

            _editorsUpdaterServiceMock
                .Setup(x => x.ChangeHtmlImagesFolder(It.IsAny<string>()))
                .ReturnsAsync("clean html");

            _newsAdderRepositoryMock
                .Setup(x => x.CreateNew(It.IsAny<New>()))
                .ReturnsAsync(createdNew);

            // Act
            var result = await _service.CreateNew(request);

            // Assert
            result.Should().NotBeNull();

            _newsAdderRepositoryMock.Verify(
                x => x.CreateNew(It.Is<New>(n =>
                    n.Title == request.Title &&
                    n.StatusId == request.StatusId &&
                    n.Description == "clean html"
                )),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_NEWS),
                Times.Once);
        }

        [Fact]
        public async Task CreateNew_Should_Throw_When_StatusId_Is_Empty()
        {
            // Arrange
            var request = _fixture.Build<NewRequestDTO>()
                .With(x => x.StatusId, Guid.Empty)
                .Without(x => x.File)
                .Create();

            // Act
            Func<Task> act = async () => await _service.CreateNew(request);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("StatusId inválido.");

            _newsAdderRepositoryMock.Verify(
                x => x.CreateNew(It.IsAny<New>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateNew_Should_Throw_When_Status_Not_Found()
        {
            // Arrange
            var request = _fixture.Build<NewRequestDTO>()
                .With(x => x.StatusId, Guid.NewGuid())
                .Without(x => x.File)
                .Create();

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))!
                .ReturnsAsync((StatusResponseDTO?)null);

            // Act
            Func<Task> act = async () => await _service.CreateNew(request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();

            _newsAdderRepositoryMock.Verify(
                x => x.CreateNew(It.IsAny<New>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateNew_Should_Throw_When_Create_Fails()
        {
            // Arrange
            var request = _fixture.Build<NewRequestDTO>()
                .With(x => x.StatusId, Guid.NewGuid())
                .Without(x => x.File)
                .Create();

            var status = _fixture.Build<Status>()
                .With(x => x.Id, request.StatusId)
                .Create();

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock
                .Setup(x => x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _editorsUpdaterServiceMock
                .Setup(x => x.ChangeHtmlImagesFolder(It.IsAny<string>()))
                .ReturnsAsync("html");

            _newsAdderRepositoryMock
                .Setup(x => x.CreateNew(It.IsAny<New>()))!
                .ReturnsAsync((New?)null);

            // Act
            Func<Task> act = async () => await _service.CreateNew(request);

            // Assert
            await act.Should()
                .ThrowAsync<CreateObjectException>();
        }

        [Fact]
        public async Task CreateNew_Should_Save_Image_When_File_Exists()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(100);

            var request = _fixture.Build<NewRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .With(x => x.StatusId, Guid.NewGuid())
                .Create();

            var status = _fixture.Build<Status>()
                .With(x => x.Id, request.StatusId)
                .Create();

            var createdNew = _fixture.Create<New>();

            _fileSystemsMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<IFormFile>(),
                    "noticias",
                    It.IsAny<string>()))
                .ReturnsAsync("https://server/noticias/image.jpg");

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock
                .Setup(x => x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _editorsUpdaterServiceMock
                .Setup(x => x.ChangeHtmlImagesFolder(It.IsAny<string>()))
                .ReturnsAsync("html");

            _newsAdderRepositoryMock
                .Setup(x => x.CreateNew(It.IsAny<New>()))
                .ReturnsAsync(createdNew);

            // Act
            await _service.CreateNew(request);

            // Assert
            _fileSystemsMock.Verify(
                x => x.SaveAsync(
                    It.IsAny<IFormFile>(),
                    "noticias",
                    It.IsAny<string>()),
                Times.Once);

            _newsAdderRepositoryMock.Verify(
                x => x.CreateNew(It.Is<New>(n =>
                    n.FileName == "image.jpg" &&
                    n.FilePath == "noticias/image.jpg"
                )),
                Times.Once);
        }
    }
}
