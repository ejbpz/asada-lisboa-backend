using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Services.Configurations;
using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Configurations;

namespace AsadaLisboaBackend.Tests.Configurations
{
    public class ConfigurationsUpdaterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<ILogger<ConfigurationsUpdaterService>> _loggerMock;
        private readonly Mock<IConfigurationsUpdaterRepository> _configurationsUpdaterRepositoryMock;

        private readonly ConfigurationsUpdaterService _service;

        public ConfigurationsUpdaterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();

            _loggerMock = new Mock<ILogger<ConfigurationsUpdaterService>>();

            _configurationsUpdaterRepositoryMock = new Mock<IConfigurationsUpdaterRepository>();

            _service = new ConfigurationsUpdaterService(
                _configurationsUpdaterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task UpdateConfiguration_Should_Update_Configuration_Successfully()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Build<ConfigurationsRequestDTO>()
                .With(x => x.Order, 1)
                .With(x => x.Value, "Misión")
                .With(x => x.SettingType, "Brindar el servicio de agua potable......")
                .Create();

            var updatedConfiguration = _fixture.Build<VisualSetting>()
                .With(x => x.Id, id)
                .With(x => x.Order, request.Order)
                .With(x => x.Value, request.Value)
                .With(x => x.SettingType, request.SettingType)
                .Create();

            _configurationsUpdaterRepositoryMock
                .Setup(x => x.UpdateConfiguration(id, request))
                .ReturnsAsync(updatedConfiguration);

            // Act
            var result = await _service.UpdateConfiguration(id, request);

            // Assert
            result.Should().NotBeNull();

            result.Order.Should().Be(request.Order);
            result.Value.Should().Be(request.Value);
            result.SettingType.Should().Be(request.SettingType);

            _configurationsUpdaterRepositoryMock.Verify(
                x => x.UpdateConfiguration(id, request),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(Constants.CACHE_CONFIGURATIONS, id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_CONFIGURATIONS),
                Times.Once);
        }

        [Fact]
        public async Task UpdateConfiguration_Should_Throw_When_Configuration_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Create<ConfigurationsRequestDTO>();

            _configurationsUpdaterRepositoryMock
                .Setup(x => x.UpdateConfiguration(id, request))
                .ReturnsAsync((VisualSetting)null!);

            // Act
            Func<Task> act = async () => await _service.UpdateConfiguration(id, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage($"No se encontró configuración para actualizar con Id: {id}");

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(It.IsAny<string>(), It.IsAny<Guid>()),
                Times.Never);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateConfiguration_Should_Call_Logger_When_Success()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Create<ConfigurationsRequestDTO>();

            var updatedConfiguration = _fixture.Build<VisualSetting>()
                .With(x => x.Id, id)
                .Create();

            _configurationsUpdaterRepositoryMock
                .Setup(x => x.UpdateConfiguration(id, request))
                .ReturnsAsync(updatedConfiguration);

            // Act
            await _service.UpdateConfiguration(id, request);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task UpdateConfiguration_Should_Call_Logger_When_NotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Create<ConfigurationsRequestDTO>();

            _configurationsUpdaterRepositoryMock
                .Setup(x => x.UpdateConfiguration(id, request))
                .ReturnsAsync((VisualSetting)null!);

            // Act
            Func<Task> act = async () => await _service.UpdateConfiguration(id, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }
    }
}