using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Images;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Services.Images
{
    public class ImagesGetterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
        private readonly Mock<IImagesGetterRepository> _imagesGetterRepositoryMock = new Mock<IImagesGetterRepository>();

        private readonly ImagesGetterService _service;

        public ImagesGetterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _service = new ImagesGetterService(
                imagesGetterRepository: _imagesGetterRepositoryMock.Object,
                memoryCachesService: _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task GetImages_ShouldReturnPagedImages()
        {
            // Arrange
            var request = _fixture.Create<SearchSortRequestDTO>();

            var expectedResponse = _fixture.Create<PageResponseDTO<ImageMinimalResponseDTO>>();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList(
                    Constants.CACHE_IMAGES,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<ImageMinimalResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.GetImages(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCacheList(
                    Constants.CACHE_IMAGES,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<ImageMinimalResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Fact]
        public async Task GetImage_ShouldReturnImage()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Build<Image>()
                .With(x => x.Id, id)
                .Create();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache(
                    $"{Constants.CACHE_IMAGES}_{id}",
                    It.IsAny<Func<Task<Image>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(image);

            // Act
            var result = await _service.GetImage(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCache(
                    $"{Constants.CACHE_IMAGES}_{id}",
                    It.IsAny<Func<Task<Image>>>(),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Fact]
        public async Task GetImageBySlug_ShouldReturnImage()
        {
            // Arrange
            var slug = "mi-imagen";

            var image = _fixture.Build<Image>()
                .With(x => x.Slug, slug)
                .Create();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache(
                    $"{Constants.CACHE_IMAGES}_{slug}",
                    It.IsAny<Func<Task<Image>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(image);

            // Act
            var result = await _service.GetImageBySlug(slug);

            // Assert
            result.Should().NotBeNull();
            result.Slug.Should().Be(slug);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCache(
                    $"{Constants.CACHE_IMAGES}_{slug}",
                    It.IsAny<Func<Task<Image>>>(),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Fact]
        public async Task GetImages_ShouldCallRepositoryThroughCacheFactory()
        {
            // Arrange
            var request = _fixture.Create<SearchSortRequestDTO>();

            var expectedResponse = _fixture.Create<PageResponseDTO<ImageMinimalResponseDTO>>();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList(
                    Constants.CACHE_IMAGES,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<ImageMinimalResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .Returns<string, object, Func<Task<PageResponseDTO<ImageMinimalResponseDTO>>>, TimeSpan>(
                    async (_, _, create, _) => await create());

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImages(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.GetImages(request);

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);

            _imagesGetterRepositoryMock.Verify(
                x => x.GetImages(request),
                Times.Once);
        }

        [Fact]
        public async Task GetImage_ShouldCallRepositoryThroughCacheFactory()
        {
            // Arrange
            var id = Guid.NewGuid();

            var image = _fixture.Build<Image>()
                .With(x => x.Id, id)
                .Create();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache(
                    $"{Constants.CACHE_IMAGES}_{id}",
                    It.IsAny<Func<Task<Image>>>(),
                    It.IsAny<TimeSpan>()))
                .Returns<string, Func<Task<Image>>, TimeSpan>(
                    async (_, create, _) => await create());

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImage(id))
                .ReturnsAsync(image);

            // Act
            var result = await _service.GetImage(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);

            _imagesGetterRepositoryMock.Verify(
                x => x.GetImage(id),
                Times.Once);
        }

        [Fact]
        public async Task GetImageBySlug_ShouldCallRepositoryThroughCacheFactory()
        {
            // Arrange
            var slug = "imagen-test";

            var image = _fixture.Build<Image>()
                .With(x => x.Slug, slug)
                .Create();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache(
                    $"{Constants.CACHE_IMAGES}_{slug}",
                    It.IsAny<Func<Task<Image>>>(),
                    It.IsAny<TimeSpan>()))
                .Returns<string, Func<Task<Image>>, TimeSpan>(
                    async (_, create, _) => await create());

            _imagesGetterRepositoryMock
                .Setup(x => x.GetImageBySlug(slug))
                .ReturnsAsync(image);

            // Act
            var result = await _service.GetImageBySlug(slug);

            // Assert
            result.Should().NotBeNull();
            result.Slug.Should().Be(slug);

            _imagesGetterRepositoryMock.Verify(
                x => x.GetImageBySlug(slug),
                Times.Once);
        }
    }
}