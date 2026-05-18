using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.News;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.News
{
    public class NewsGetterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<INewsGetterRepository> _newsGetterRepositoryMock;

        private readonly NewsGetterService _service;

        public NewsGetterServiceTest()
        {
            _fixture = new Fixture();

            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
            _newsGetterRepositoryMock = new Mock<INewsGetterRepository>();

            _service = new NewsGetterService(
                _newsGetterRepositoryMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task GetNew_Should_Return_Cached_New()
        {
            // Arrange
            var id = Guid.NewGuid();

            var expected = _fixture.Create<NewResponseDTO>();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache<NewResponseDTO>(
                    $"{Constants.CACHE_NEWS}_{id}",
                    It.IsAny<Func<Task<NewResponseDTO>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetNew(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCache<NewResponseDTO>(
                    $"{Constants.CACHE_NEWS}_{id}",
                    It.IsAny<Func<Task<NewResponseDTO>>>(),
                    TimeSpan.FromMinutes(5)),
                Times.Once);
        }

        [Fact]
        public async Task GetNewBySlug_Should_Return_Cached_New()
        {
            // Arrange
            var slug = "my-news";

            var expected = _fixture.Create<NewResponseDTO>();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache<NewResponseDTO>(
                    $"{Constants.CACHE_NEWS}_{slug}",
                    It.IsAny<Func<Task<NewResponseDTO>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetNewBySlug(slug);

            // Assert
            result.Should().BeEquivalentTo(expected);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCache<NewResponseDTO>(
                    $"{Constants.CACHE_NEWS}_{slug}",
                    It.IsAny<Func<Task<NewResponseDTO>>>(),
                    TimeSpan.FromMinutes(5)),
                Times.Once);
        }

        [Fact]
        public async Task GetNews_Should_Return_Cached_List()
        {
            // Arrange
            var request = _fixture.Create<SearchSortRequestDTO>();

            var expected = _fixture.Create<PageResponseDTO<NewMinimalResponseDTO>>();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<NewMinimalResponseDTO>>(
                    Constants.CACHE_NEWS,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<NewMinimalResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetNews(request);

            // Assert
            result.Should().BeEquivalentTo(expected);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCacheList<PageResponseDTO<NewMinimalResponseDTO>>(
                    Constants.CACHE_NEWS,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<NewMinimalResponseDTO>>>>(),
                    TimeSpan.FromMinutes(5)),
                Times.Once);
        }

        [Fact]
        public async Task GetRecommendedNews_Should_Return_Cached_List()
        {
            // Arrange
            var slug = "test-news";

            var expected = _fixture.Create<List<NewMinimalResponseDTO>>();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<List<NewMinimalResponseDTO>>(
                    $"{Constants.CACHE_NEWS}-{slug}",
                    slug,
                    It.IsAny<Func<Task<List<NewMinimalResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetRecommendedNews(slug);

            // Assert
            result.Should().BeEquivalentTo(expected);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCacheList<List<NewMinimalResponseDTO>>(
                    $"{Constants.CACHE_NEWS}-{slug}",
                    slug,
                    It.IsAny<Func<Task<List<NewMinimalResponseDTO>>>>(),
                    TimeSpan.FromMinutes(5)),
                Times.Once);
        }

        [Fact]
        public async Task GetNew_Should_Call_Repository_Through_Cache_Factory()
        {
            // Arrange
            var id = Guid.NewGuid();

            var expected = _fixture.Create<NewResponseDTO>();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache<NewResponseDTO>(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<NewResponseDTO>>>(),
                    It.IsAny<TimeSpan>()))
                .Returns<string, Func<Task<NewResponseDTO>>, TimeSpan>(
                    async (_, create, _) => await create());

            _newsGetterRepositoryMock
                .Setup(x => x.GetNew(id))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetNew(id);

            // Assert
            result.Should().BeEquivalentTo(expected);

            _newsGetterRepositoryMock.Verify(
                x => x.GetNew(id),
                Times.Once);
        }
    }
}
