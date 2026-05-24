using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Services.Statuses;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Statuses
{
    public class StatusesGetterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<ILogger<StatusesGetterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<IStatusesGetterRepository> _statusesGetterRepositoryMock;

        private readonly StatusesGetterService _service;

        public StatusesGetterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _loggerMock = new Mock<ILogger<StatusesGetterService>>();
            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
            _statusesGetterRepositoryMock = new Mock<IStatusesGetterRepository>();

            _service = new StatusesGetterService(
                _statusesGetterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task GetStatuses_Should_Return_Statuses_Successfully()
        {
            // Arrange
            var statuses = _fixture
                .CreateMany<StatusResponseDTO>(3)
                .ToList();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache<List<StatusResponseDTO>>(
                    Constants.CACHE_CATEGORIES,
                    It.IsAny<Func<Task<List<StatusResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(statuses);

            // Act
            var result = await _service.GetStatuses();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(statuses);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCache<List<StatusResponseDTO>>(
                    Constants.CACHE_CATEGORIES,
                    It.IsAny<Func<Task<List<StatusResponseDTO>>>>(),
                    It.Is<TimeSpan>(t => t == TimeSpan.FromMinutes(5))),
                Times.Once);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetStatuses_Should_Throw_Exception_When_Cache_Fails()
        {
            // Arrange
            var exception = new Exception("Cache error");

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache<List<StatusResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<List<StatusResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ThrowsAsync(exception);

            // Act
            Func<Task> act = async () => await _service.GetStatuses();

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Cache error");

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetStatuses_Should_Call_Repository_Through_Cache()
        {
            // Arrange
            var statuses = _fixture
                .CreateMany<StatusResponseDTO>(2)
                .ToList();

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatuses())
                .ReturnsAsync(statuses);

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache<List<StatusResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<List<StatusResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .Returns<string, Func<Task<List<StatusResponseDTO>>>, TimeSpan>(
                    async (_, create, _) => await create());

            // Act
            var result = await _service.GetStatuses();

            // Assert
            result.Should().BeEquivalentTo(statuses);

            _statusesGetterRepositoryMock.Verify(
                x => x.GetStatuses(),
                Times.Once);
        }
    }
}