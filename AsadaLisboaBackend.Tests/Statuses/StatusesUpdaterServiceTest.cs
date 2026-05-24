using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Services.Statuses;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Statuses
{
    public class StatusesUpdaterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<ILogger<StatusesUpdaterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<IStatusesGetterRepository> _statusesGetterRepositoryMock;
        private readonly Mock<IStatusesUpdaterRepository> _statusesUpdaterRepositoryMock;

        private readonly StatusesUpdaterService _service;

        public StatusesUpdaterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _loggerMock = new Mock<ILogger<StatusesUpdaterService>>();
            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
            _statusesGetterRepositoryMock = new Mock<IStatusesGetterRepository>();
            _statusesUpdaterRepositoryMock = new Mock<IStatusesUpdaterRepository>();

            _service = new StatusesUpdaterService(
                _statusesGetterRepositoryMock.Object,
                _statusesUpdaterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task ChangeStatus_Should_Change_Status_Successfully()
        {
            // Arrange
            var objectId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var objectType = ObjectTypeEnum.New;

            var status = _fixture.Build<StatusResponseDTO>()
                .With(x => x.Id, statusId)
                .Create();

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(statusId))
                .ReturnsAsync(status);

            _statusesUpdaterRepositoryMock
                .Setup(x => x.ChangeStatus(objectId, statusId, objectType))
                .Returns(Task.CompletedTask);

            // Act
            await _service.ChangeStatus(objectId, statusId, objectType);

            // Assert
            _statusesGetterRepositoryMock.Verify(
                x => x.GetStatus(statusId),
                Times.Once);

            _statusesUpdaterRepositoryMock.Verify(
                x => x.ChangeStatus(objectId, statusId, objectType),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(Constants.CACHE_STATUSES, objectId),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_STATUSES),
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
        public async Task ChangeStatus_Should_Throw_When_Status_Not_Exists()
        {
            // Arrange
            var objectId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var objectType = ObjectTypeEnum.New;

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(statusId))
                .ReturnsAsync((StatusResponseDTO)null!);

            // Act
            Func<Task> act = async () =>
                await _service.ChangeStatus(objectId, statusId, objectType);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("El estado no existe");

            _statusesUpdaterRepositoryMock.Verify(
                x => x.ChangeStatus(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<ObjectTypeEnum>()),
                Times.Never);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task ChangeStatus_Should_Throw_When_Repository_Fails()
        {
            // Arrange
            var objectId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var objectType = ObjectTypeEnum.New;

            var status = _fixture.Build<StatusResponseDTO>()
                .With(x => x.Id, statusId)
                .Create();

            var exception = new Exception("Database error");

            _statusesGetterRepositoryMock
                .Setup(x => x.GetStatus(statusId))
                .ReturnsAsync(status);

            _statusesUpdaterRepositoryMock
                .Setup(x => x.ChangeStatus(objectId, statusId, objectType))
                .ThrowsAsync(exception);

            // Act
            Func<Task> act = async () =>
                await _service.ChangeStatus(objectId, statusId, objectType);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database error");

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(
                    It.IsAny<string>(),
                    It.IsAny<Guid>()),
                Times.Never);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(
                    It.IsAny<string>()),
                Times.Never);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}