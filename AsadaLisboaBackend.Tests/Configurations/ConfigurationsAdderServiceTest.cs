using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.Services.Configurations;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Configurations;

namespace AsadaLisboaBackend.Tests.Configurations
{
    public class ConfigurationsAdderServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IConfigurationsAdderRepository> _configurationsAdderRepositoryMock;
        private readonly Mock<ILogger<ConfigurationsAdderService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly ConfigurationsAdderService _service;

        public ConfigurationsAdderServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _configurationsAdderRepositoryMock = new Mock<IConfigurationsAdderRepository>();
            _loggerMock = new Mock<ILogger<ConfigurationsAdderService>>();
            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();

            _service = new ConfigurationsAdderService(
                _configurationsAdderRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateConfiguration_Should_Create_Configuration_Successfully()
        {
            // Arrange
            var request = _fixture.Build<ConfigurationsRequestDTO>()
                .With(x => x.Order, 1)
                .With(x => x.Value, "Misión")
                .With(x => x.SettingType, "Brindar el servicio de agua potable......")
                .Create();

            var createdConfiguration = _fixture.Build<VisualSetting>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.Order, request.Order)
                .With(x => x.Value, request.Value)
                .With(x => x.SettingType, request.SettingType)
                .Create();

            _configurationsAdderRepositoryMock
                .Setup(x => x.CreateConfiguration(It.IsAny<VisualSetting>()))
                .ReturnsAsync(createdConfiguration);

            // Act
            var result = await _service.CreateConfiguration(request);

            // Assert
            result.Should().NotBeNull();

            result.Order.Should().Be(request.Order);
            result.Value.Should().Be(request.Value);
            result.SettingType.Should().Be(request.SettingType);

            _configurationsAdderRepositoryMock.Verify(
                x => x.CreateConfiguration(It.Is<VisualSetting>(v =>
                    v.Order == request.Order &&
                    v.Value == request.Value &&
                    v.SettingType == request.SettingType
                )),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_CONFIGURATIONS),
                Times.Once);
        }

        [Fact]
        public async Task CreateConfiguration_Should_Throw_When_Repository_Fails()
        {
            // Arrange
            var request = _fixture.Build<ConfigurationsRequestDTO>()
                .Create();

            _configurationsAdderRepositoryMock
                .Setup(x => x.CreateConfiguration(It.IsAny<VisualSetting>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () =>
                await _service.CreateConfiguration(request);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database error");

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(It.IsAny<string>()),
                Times.Never);

            _configurationsAdderRepositoryMock.Verify(
                x => x.CreateConfiguration(It.IsAny<VisualSetting>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateConfiguration_Should_Call_Logger_When_Success()
        {
            // Arrange
            var request = _fixture.Create<ConfigurationsRequestDTO>();

            var createdConfiguration = _fixture.Build<VisualSetting>()
                .With(x => x.Order, request.Order)
                .With(x => x.Value, request.Value)
                .With(x => x.SettingType, request.SettingType)
                .Create();

            _configurationsAdderRepositoryMock
                .Setup(x => x.CreateConfiguration(It.IsAny<VisualSetting>()))
                .ReturnsAsync(createdConfiguration);

            // Act
            await _service.CreateConfiguration(request);

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
        public async Task CreateConfiguration_Should_Call_Logger_When_Error()
        {
            // Arrange
            var request = _fixture.Create<ConfigurationsRequestDTO>();

            _configurationsAdderRepositoryMock
                .Setup(x => x.CreateConfiguration(It.IsAny<VisualSetting>()))
                .ThrowsAsync(new Exception("Internal error"));

            // Act
            Func<Task> act = async () =>
                await _service.CreateConfiguration(request);

            // Assert
            await act.Should().ThrowAsync<Exception>();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }
    }
}