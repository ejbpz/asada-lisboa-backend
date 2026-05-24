using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.RepositoryContracts.Configurations;
using AsadaLisboaBackend.Services.Configurations;

namespace AsadaLisboaBackend.Tests.Configurations
{
    public class ConfigurationsGetterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<ILogger<ConfigurationsGetterService>> _loggerMock;
        private readonly Mock<IConfigurationsGetterRepository> _configurationsGetterRepositoryMock;

        private readonly ConfigurationsGetterService _service;

        public ConfigurationsGetterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
            _loggerMock = new Mock<ILogger<ConfigurationsGetterService>>();
            _configurationsGetterRepositoryMock = new Mock<IConfigurationsGetterRepository>();

            _service = new ConfigurationsGetterService(
                _configurationsGetterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task GetConfigurations_Should_Return_Configurations_Successfully()
        {
            // Arrange
            var request = _fixture.Build<SearchSortRequestDTO>()
                .With(x => x.Take, 10)
                .Create();

            var configurations = _fixture
                .CreateMany<ConfigurationResponseDTO>(5)
                .ToList();

            var pageResponse = new PageResponseDTO<ConfigurationResponseDTO>
            {
                Data = configurations,
                Total = configurations.Count
            };

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<ConfigurationResponseDTO>>(
                    Constants.CACHE_CONFIGURATIONS,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<ConfigurationResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(pageResponse);

            // Act
            var result = await _service.GetConfigurations(request);

            // Assert
            result.Should().NotBeNull();
            result.Total.Should().Be(configurations.Count);
            result.Data.Should().HaveCount(5);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCacheList<PageResponseDTO<ConfigurationResponseDTO>>(
                    Constants.CACHE_CONFIGURATIONS,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<ConfigurationResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Fact]
        public async Task GetConfigurations_Should_Call_Logger_When_Success()
        {
            // Arrange
            var request = _fixture.Build<SearchSortRequestDTO>()
                .With(x => x.Take, 10)
                .Create();

            var pageResponse = new PageResponseDTO<ConfigurationResponseDTO>
            {
                Data = new List<ConfigurationResponseDTO>(),
                Total = 0
            };

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<ConfigurationResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<Func<Task<PageResponseDTO<ConfigurationResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(pageResponse);

            // Act
            await _service.GetConfigurations(request);

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
        public async Task GetConfigurations_Should_Throw_When_Cache_Fails()
        {
            // Arrange
            var request = _fixture.Build<SearchSortRequestDTO>()
                .With(x => x.Take, 10)
                .Create();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<ConfigurationResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<Func<Task<PageResponseDTO<ConfigurationResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ThrowsAsync(new Exception("Cache error"));

            // Act
            Func<Task> act = async () =>
                await _service.GetConfigurations(request);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Cache error");

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task GetConfigurations_Should_Call_Repository_Through_Cache_Create_Function()
        {
            // Arrange
            var request = _fixture.Create<SearchSortRequestDTO>();

            var expectedResponse = new PageResponseDTO<ConfigurationResponseDTO>
            {
                Data = _fixture.CreateMany<ConfigurationResponseDTO>(3).ToList(),
                Total = 3
            };

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<ConfigurationResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<Func<Task<PageResponseDTO<ConfigurationResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .Returns<string, object, Func<Task<PageResponseDTO<ConfigurationResponseDTO>>>, TimeSpan>(
                    async (resource, req, create, time) =>
                    {
                        return await create();
                    });

            _configurationsGetterRepositoryMock
                .Setup(x => x.GetConfigurations(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.GetConfigurations(request);

            // Assert
            result.Should().NotBeNull();
            result.Total.Should().Be(3);

            _configurationsGetterRepositoryMock.Verify(
                x => x.GetConfigurations(request),
                Times.Once);
        }

    }
}
