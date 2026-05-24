using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;

using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.Services.AboutUsSections;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Tests.AboutUsSection
{
    public class AboutUsSectionsGetterServiceTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<ILogger<AboutUsSectionsGetterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<IAboutUsSectionsGetterRepository> _aboutUsSectionsGetterRepositoryMock;

        private readonly AboutUsSectionsGetterService _service;


        public AboutUsSectionsGetterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors
                .Add(new OmitOnRecursionBehavior());

            _loggerMock =
                new Mock<ILogger<AboutUsSectionsGetterService>>();

            _memoryCachesServiceMock =
                new Mock<IMemoryCachesService>();

            _aboutUsSectionsGetterRepositoryMock =
                new Mock<IAboutUsSectionsGetterRepository>();

            _service = new AboutUsSectionsGetterService(
                _aboutUsSectionsGetterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task GetAboutUsSections_Should_Return_PageResponse_Successfully()
        {
            // Arrange
            var request = _fixture.Build<SearchSortRequestDTO>()
                .With(x => x.Take, 10)
                .Create();

            var response = new PageResponseDTO<AboutUsResponseDTO>
            {
                Total = 5,
                Data = _fixture.CreateMany<AboutUsResponseDTO>(5).ToList()
            };

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<AboutUsResponseDTO>>(
                    Constants.CACHE_ABOUT_US,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<AboutUsResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(response);

            // Act
            var result =
                await _service.GetAboutUsSections(request);

            // Assert
            result.Should().NotBeNull();

            result.Total.Should().Be(5);

            result.Data.Should().HaveCount(5);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCacheList<PageResponseDTO<AboutUsResponseDTO>>(
                    Constants.CACHE_ABOUT_US,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<AboutUsResponseDTO>>>>(),
                    It.Is<TimeSpan>(t => t == TimeSpan.FromMinutes(5))),
                Times.Once);
        }

        [Fact]
        public async Task GetAboutUsSections_Should_Throw_And_LogError_When_Exception_Occurs()
        {
            // Arrange
            var request = _fixture.Build<SearchSortRequestDTO>()
                .With(x => x.Take, 10)
                .Create();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<AboutUsResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<SearchSortRequestDTO>(),
                    It.IsAny<Func<Task<PageResponseDTO<AboutUsResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ThrowsAsync(new Exception("Cache error"));

            // Act
            Func<Task> act = async () =>
                await _service.GetAboutUsSections(request);

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
                Times.Once);
        }

    }
}
