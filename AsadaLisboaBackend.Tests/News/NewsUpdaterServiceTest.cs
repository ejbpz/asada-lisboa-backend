using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Services.News;
using AsadaLisboaBackend.Utils;
using AutoFixture;
using Elastic.Clients.Elasticsearch;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace AsadaLisboaBackend.Tests.News
{
    public class NewsUpdaterServiceTests
    {
        private readonly Fixture _fixture;

        private readonly Mock<IFileSystemsManager> _fileSystemsMock;
        private readonly Mock<ILogger<NewsUpdaterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<INewsGetterRepository> _newsGetterRepositoryMock;
        private readonly Mock<IEditorsUpdaterService> _editorsUpdaterServiceMock;
        private readonly Mock<IEditorsDeleterService> _editorsDeleterServiceMock;
        private readonly Mock<INewsUpdaterRepository> _newsUpdaterRepositoryMock;
        private readonly Mock<ICategoriesGetterService> _categoriesGetterServiceMock;
        private readonly Mock<IStatusesGetterRepository> _statusesGetterRepositoryMock;

        private readonly Mock<ElasticsearchClient> _elasticMock;

        private readonly NewsUpdaterService _service;

        public NewsUpdaterServiceTests()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fileSystemsMock = new Mock<IFileSystemsManager>();
            _loggerMock = new Mock<ILogger<NewsUpdaterService>>();
            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
            _newsGetterRepositoryMock = new Mock<INewsGetterRepository>();
            _editorsUpdaterServiceMock = new Mock<IEditorsUpdaterService>();
            _editorsDeleterServiceMock = new Mock<IEditorsDeleterService>();
            _newsUpdaterRepositoryMock = new Mock<INewsUpdaterRepository>();
            _categoriesGetterServiceMock = new Mock<ICategoriesGetterService>();
            _statusesGetterRepositoryMock = new Mock<IStatusesGetterRepository>();

            _elasticMock = new Mock<ElasticsearchClient>();

            _service = new NewsUpdaterService(
                _newsUpdaterRepositoryMock.Object,
                _newsGetterRepositoryMock.Object,
                _editorsUpdaterServiceMock.Object,
                _editorsDeleterServiceMock.Object,
                _statusesGetterRepositoryMock.Object,
                _categoriesGetterServiceMock.Object,
                _fileSystemsMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object,
                _elasticMock.Object
            );
        }

        [Fact]
        public async Task UpdateNew_Should_Update_Successfully()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Build<NewRequestDTO>()
                .With(x => x.StatusId, Guid.NewGuid())
                .Without(x => x.File)
                .Create();

            var existingNew = _fixture.Build<New>()
                .With(x => x.Id, id)
                .Create();

            var status = _fixture.Build<Status>()
                .With(x => x.Id, request.StatusId)
                .With(x => x.Name, "Publicado")
                .Create();

            var categories = _fixture.CreateMany<Category>(2).ToList();

            var updatedNew = _fixture.Build<New>()
                .With(x => x.Id, id)
                .Create();

            _newsGetterRepositoryMock
                .Setup(x => x.GetNew(id))
                .ReturnsAsync(existingNew.ToNewResponseDTO());

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock
                .Setup(x => x.ToCreateCategories(request.Categories))
                .ReturnsAsync(categories.ToHashSet());

            _editorsUpdaterServiceMock
                .Setup(x => x.ChangeHtmlImagesFolder(It.IsAny<string>()))
                .ReturnsAsync("clean html");

            _newsUpdaterRepositoryMock
                .Setup(x => x.UpdateNew(id, It.IsAny<New>()))
                .ReturnsAsync(updatedNew);

            // Act
            var result = await _service.UpdateNew(id, request);

            // Assert
            result.Should().NotBeNull();

            _newsUpdaterRepositoryMock.Verify(
                x => x.UpdateNew(id, It.Is<New>(n =>
                    n.Title == request.Title &&
                    n.Description == "clean html" &&
                    n.StatusId == request.StatusId
                )),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(Constants.CACHE_NEWS, updatedNew.Id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_NEWS),
                Times.Once);
        }

        [Fact]
        public async Task UpdateNew_Should_Throw_When_New_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Build<NewRequestDTO>()
                .Without(x => x.File)
                .Create();

            _newsGetterRepositoryMock
                .Setup(x => x.GetNew(id))!
                .ReturnsAsync((NewResponseDTO?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateNew(id, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateNew_Should_Throw_When_StatusId_Is_Empty()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Build<NewRequestDTO>()
                .With(x => x.StatusId, Guid.Empty)
                .Without(x => x.File)
                .Create();

            var existingNew = _fixture.Create<New>();

            _newsGetterRepositoryMock
                .Setup(x => x.GetNew(id))
                .ReturnsAsync(existingNew.ToNewResponseDTO());

            // Act
            Func<Task> act = async () => await _service.UpdateNew(id, request);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateNew_Should_Throw_When_Status_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Build<NewRequestDTO>()
                .With(x => x.StatusId, Guid.NewGuid())
                .Without(x => x.File)
                .Create();

            var existingNew = _fixture.Create<New>();

            _newsGetterRepositoryMock
                .Setup(x => x.GetNew(id))
                .ReturnsAsync(existingNew.ToNewResponseDTO());

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))!
                .ReturnsAsync((StatusResponseDTO?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateNew(id, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateNew_Should_Throw_When_Update_Fails()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Build<NewRequestDTO>()
                .With(x => x.StatusId, Guid.NewGuid())
                .Without(x => x.File)
                .Create();

            var existingNew = _fixture.Create<New>();

            var status = _fixture.Build<Status>()
                .With(x => x.Id, request.StatusId)
                .Create();

            _newsGetterRepositoryMock
                .Setup(x => x.GetNew(id))
                .ReturnsAsync(existingNew.ToNewResponseDTO());

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock
                .Setup(x => x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _editorsUpdaterServiceMock
                .Setup(x => x.ChangeHtmlImagesFolder(It.IsAny<string>()))
                .ReturnsAsync("html");

            _newsUpdaterRepositoryMock
                .Setup(x => x.UpdateNew(id, It.IsAny<New>()))!
                .ReturnsAsync((New?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateNew(id, request);

            // Assert
            await act.Should()
                .ThrowAsync<CreateObjectException>();
        }

        [Fact]
        public async Task UpdateNew_Should_Replace_Image_When_New_File_Exists()
        {
            // Arrange
            var id = Guid.NewGuid();

            var fileMock = new Mock<IFormFile>();

            var request = _fixture.Build<NewRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .With(x => x.StatusId, Guid.NewGuid())
                .Without(x => x.File)
                .Create();

            var existingNew = _fixture.Build<New>()
                .With(x => x.FileName, "old.jpg")
                .Create();

            var status = _fixture.Build<Status>()
                .With(x => x.Id, request.StatusId)
                .With(x => x.Name, "Publicado")
                .Create();

            var updatedNew = _fixture.Create<New>();

            _newsGetterRepositoryMock
                .Setup(x => x.GetNew(id))
                .ReturnsAsync(existingNew.ToNewResponseDTO());

            _fileSystemsMock
                .Setup(x => x.SaveAsync(
                    fileMock.Object,
                    "noticias",
                    It.IsAny<string>()))
                .ReturnsAsync("https://cdn/new.jpg");

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(request.StatusId))
                .ReturnsAsync(status.ToStatusResponseDTO());

            _categoriesGetterServiceMock
                .Setup(x => x.ToCreateCategories(It.IsAny<List<CategoryRequestDTO>>()))
                .ReturnsAsync(new HashSet<Category>());

            _editorsUpdaterServiceMock
                .Setup(x => x.ChangeHtmlImagesFolder(It.IsAny<string>()))
                .ReturnsAsync("html");

            _newsUpdaterRepositoryMock
                .Setup(x => x.UpdateNew(id, It.IsAny<New>()))
                .ReturnsAsync(updatedNew);

            // Act
            await _service.UpdateNew(id, request);

            // Assert

            _fileSystemsMock.Verify(
                x => x.SaveAsync(
                    fileMock.Object,
                    "noticias",
                    It.IsAny<string>()),
                Times.Once);

            _fileSystemsMock.Verify(
                x => x.DeleteAsync("old.jpg", "noticias"),
                Times.Once);

            _newsUpdaterRepositoryMock.Verify(
                x => x.UpdateNew(id, It.Is<New>(n =>
                    n.FileName == "new.jpg" &&
                    n.FilePath == "noticias/new.jpg"
                )),
                Times.Once);
        }
    }
}