using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;
using Elastic.Clients.Elasticsearch;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.News;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.News
{
    public class NewsDeleterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<ILogger<NewsDeleterService>> _loggerMock;
        private readonly Mock<ElasticsearchClient> _elasticSearchClientMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<INewsGetterRepository> _newsGetterRepositoryMock;
        private readonly Mock<INewsDeleterRepository> _newsDeleterRepositoryMock;
        private readonly Mock<IEditorsDeleterService> _editorsDeleterServiceMock;

        private readonly NewsDeleterService _service;

        public NewsDeleterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _loggerMock = new Mock<ILogger<NewsDeleterService>>();
            _elasticSearchClientMock = new Mock<ElasticsearchClient>();
            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
            _newsGetterRepositoryMock = new Mock<INewsGetterRepository>();
            _newsDeleterRepositoryMock = new Mock<INewsDeleterRepository>();
            _editorsDeleterServiceMock = new Mock<IEditorsDeleterService>();

            _service = new NewsDeleterService(
                _newsDeleterRepositoryMock.Object,
                _editorsDeleterServiceMock.Object,
                _newsGetterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object,
                _elasticSearchClientMock.Object
            );
        }

        [Fact]
        public async Task DeleteNew_Should_Delete_Successfully()
        {
            // Arrange
            var id = Guid.NewGuid();

            var existingNew = _fixture.Build<New>()
                .With(x => x.Id, id)
                .With(x => x.FileName, "image.jpg")
                .With(x => x.Description, "<img src='test.jpg'>")
                .Create();

            _newsGetterRepositoryMock
                .Setup(x => x.GetNew(id))
                .ReturnsAsync(existingNew.ToNewResponseDTO());

            // Act
            Func<Task> act = async () => await _service.DeleteNew(id);

            // Assert
            await act.Should().NotThrowAsync();

            _editorsDeleterServiceMock.Verify(
                x => x.DeletePrincipalImage(existingNew.FileName),
                Times.Once);

            _editorsDeleterServiceMock.Verify(
                x => x.DeleteContentImages(existingNew.Description),
                Times.Once);

            _newsDeleterRepositoryMock.Verify(
                x => x.DeleteNew(id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(Constants.CACHE_NEWS, existingNew.Id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_NEWS),
                Times.Once);
        }

        [Fact]
        public async Task DeleteNew_Should_Throw_When_New_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();

            _newsGetterRepositoryMock
                .Setup(x => x.GetNew(id))!
                .ReturnsAsync((NewResponseDTO?)null);

            // Act
            Func<Task> act = async () => await _service.DeleteNew(id);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("La noticia no fue encontrada.");

            _newsDeleterRepositoryMock.Verify(
                x => x.DeleteNew(It.IsAny<Guid>()),
                Times.Never);

            _editorsDeleterServiceMock.Verify(
                x => x.DeletePrincipalImage(It.IsAny<string>()),
                Times.Never);

            _editorsDeleterServiceMock.Verify(
                x => x.DeleteContentImages(It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteNew_Should_Execute_In_Correct_Order()
        {
            // Arrange
            var id = Guid.NewGuid();

            var existingNew = _fixture.Build<New>()
                .With(x => x.Id, id)
                .Create();

            var sequence = new MockSequence();

            _newsGetterRepositoryMock
                .InSequence(sequence)
                .Setup(x => x.GetNew(id))
                .ReturnsAsync(existingNew.ToNewResponseDTO());

            _editorsDeleterServiceMock
                .InSequence(sequence)
                .Setup(x => x.DeletePrincipalImage(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _editorsDeleterServiceMock
                .InSequence(sequence)
                .Setup(x => x.DeleteContentImages(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _newsDeleterRepositoryMock
                .InSequence(sequence)
                .Setup(x => x.DeleteNew(id))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await _service.DeleteNew(id);

            // Assert
            await act.Should().NotThrowAsync();

            _newsDeleterRepositoryMock.Verify(
                x => x.DeleteNew(id),
                Times.Once);
        }

        [Fact]
        public async Task DeleteNew_Should_Stop_When_DeletePrincipalImage_Fails()
        {
            // Arrange
            var id = Guid.NewGuid();

            var existingNew = _fixture.Build<New>()
                .With(x => x.Id, id)
                .Create();

            _newsGetterRepositoryMock
                .Setup(x => x.GetNew(id))
                .ReturnsAsync(existingNew.ToNewResponseDTO());

            _editorsDeleterServiceMock
                .Setup(x => x.DeletePrincipalImage(It.IsAny<string>()))
                .ThrowsAsync(new Exception("File error"));

            // Act
            Func<Task> act = async () => await _service.DeleteNew(id);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>();

            _newsDeleterRepositoryMock.Verify(
                x => x.DeleteNew(It.IsAny<Guid>()),
                Times.Never);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteNew_Should_Remove_Cache_By_Correct_Id()
        {
            // Arrange
            var id = Guid.NewGuid();

            var existingNew = _fixture.Build<New>()
                .With(x => x.Id, id)
                .Create();

            _newsGetterRepositoryMock
                .Setup(x => x.GetNew(id))
                .ReturnsAsync(existingNew.ToNewResponseDTO());

            // Act
            Func<Task> act = async () => await _service.DeleteNew(id);

            // Assert
            await act.Should().NotThrowAsync();

            // Assert
            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(
                    Constants.CACHE_NEWS,
                    id),
                Times.Once);
        }
    }
}
