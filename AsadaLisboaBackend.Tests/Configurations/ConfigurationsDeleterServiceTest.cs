using AsadaLisboaBackend.RepositoryContracts.Configurations;
using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Services.Configurations;
using AsadaLisboaBackend.Utils;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AsadaLisboaBackend.Tests.Configurations
{
    public class ConfigurationsDeleterServiceTest
    {
            private readonly Fixture _fixture;

            private readonly Mock<IConfigurationsDeleterRepository> _configurationsDeleterRepositoryMock;
            private readonly Mock<ILogger<ConfigurationsDeleterService>> _loggerMock;
            private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

            private readonly ConfigurationsDeleterService _service;

            public ConfigurationsDeleterServiceTest()
            {
                _fixture = new Fixture();

                _fixture.Behaviors
                    .OfType<ThrowingRecursionBehavior>()
                    .ToList()
                    .ForEach(x => _fixture.Behaviors.Remove(x));

                _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                _configurationsDeleterRepositoryMock = new Mock<IConfigurationsDeleterRepository>();

                _loggerMock = new Mock<ILogger<ConfigurationsDeleterService>>();

                _memoryCachesServiceMock = new Mock<IMemoryCachesService>();

                _service = new ConfigurationsDeleterService(
                    _configurationsDeleterRepositoryMock.Object,
                    _loggerMock.Object,
                    _memoryCachesServiceMock.Object
                );
            }

            [Fact]
            public async Task UpdateConfiguration_Should_Delete_Configuration_Successfully()
            {
                // Arrange
                var id = Guid.NewGuid();

                _configurationsDeleterRepositoryMock
                    .Setup(x => x.DeleteConfiguration(id))
                    .Returns(Task.CompletedTask);

                // Act
                await _service.UpdateConfiguration(id);

                // Assert
                _configurationsDeleterRepositoryMock.Verify(
                    x => x.DeleteConfiguration(id),
                    Times.Once);

                _memoryCachesServiceMock.Verify(
                    x => x.RemoveById(Constants.CACHE_CONFIGURATIONS, id),
                    Times.Once);

                _memoryCachesServiceMock.Verify(
                    x => x.ChangeVersion(Constants.CACHE_CONFIGURATIONS),
                    Times.Once);
            }

            [Fact]
            public async Task UpdateConfiguration_Should_Call_Logger_When_Success()
            {
                // Arrange
                var id = Guid.NewGuid();

                _configurationsDeleterRepositoryMock
                    .Setup(x => x.DeleteConfiguration(id))
                    .Returns(Task.CompletedTask);

                // Act
                await _service.UpdateConfiguration(id);

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
            public async Task UpdateConfiguration_Should_Throw_When_Delete_Fails()
            {
                // Arrange
                var id = Guid.NewGuid();

                _configurationsDeleterRepositoryMock
                    .Setup(x => x.DeleteConfiguration(id))
                    .ThrowsAsync(new Exception("Delete error"));

                // Act
                Func<Task> act = async () => await _service.UpdateConfiguration(id);

                // Assert
                await act.Should()
                    .ThrowAsync<Exception>()
                    .WithMessage("Delete error");

                _memoryCachesServiceMock.Verify(
                    x => x.RemoveById(It.IsAny<string>(), It.IsAny<Guid>()),
                    Times.Never);

                _memoryCachesServiceMock.Verify(
                    x => x.ChangeVersion(It.IsAny<string>()),
                    Times.Never);
            }

            [Fact]
            public async Task UpdateConfiguration_Should_Call_Logger_When_Error()
            {
                // Arrange
                var id = Guid.NewGuid();

                _configurationsDeleterRepositoryMock
                    .Setup(x => x.DeleteConfiguration(id))
                    .ThrowsAsync(new Exception("Internal error"));

                // Act
                Func<Task> act = async () => await _service.UpdateConfiguration(id);

                // Assert
                await act.Should()
                    .ThrowAsync<Exception>();

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


