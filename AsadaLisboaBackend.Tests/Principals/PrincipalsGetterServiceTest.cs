using Moq;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils; 
using AsadaLisboaBackend.Services.Principals;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Principal;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Image; 
using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.Tests.Principals
{
    public class PrincipalsGetterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<INewsGetterService> _newsGetterServiceMock;
        private readonly Mock<IImagesGetterService> _imagesGetterServiceMock;
        private readonly Mock<IDocumentsGetterService> _documentsGetterServiceMock;
        private readonly Mock<IStatusesGetterRepository> _statusesGetterRepositoryMock;
        private readonly Mock<ILogger<PrincipalsGetterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly PrincipalsGetterService _service;

        public PrincipalsGetterServiceTest()
        {
            _fixture = new Fixture();

            // Configuración idéntica para evitar bucles por recursión de entidades
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _newsGetterServiceMock = new Mock<INewsGetterService>();
            _imagesGetterServiceMock = new Mock<IImagesGetterService>();
            _documentsGetterServiceMock = new Mock<IDocumentsGetterService>();
            _statusesGetterRepositoryMock = new Mock<IStatusesGetterRepository>();
            _loggerMock = new Mock<ILogger<PrincipalsGetterService>>();
            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();

            _service = new PrincipalsGetterService(
                _newsGetterServiceMock.Object,
                _imagesGetterServiceMock.Object,
                _documentsGetterServiceMock.Object,
                _statusesGetterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task GetPrincipalInformation_Should_Return_Public_Data_Successfully()
        {
            // Arrange
            var newsResponse = _fixture.Create<PageResponseDTO<NewMinimalResponseDTO>>();
            var imagesResponse = _fixture.Create<PageResponseDTO<ImageMinimalResponseDTO>>();
            var documentsResponse = _fixture.Create<PageResponseDTO<DocumentMinimalResponseDTO>>();

            // Validamos que el request interno configure obligatoriamente IsPublic = true
            _newsGetterServiceMock
                .Setup(x => x.GetNews(It.Is<SearchSortRequestDTO>(r => r.IsPublic == true && r.Take == 6 && r.Offset == 0)))
                .ReturnsAsync(newsResponse);

            _imagesGetterServiceMock
                .Setup(x => x.GetImages(It.Is<SearchSortRequestDTO>(r => r.IsPublic == true && r.Take == 6 && r.Offset == 0)))
                .ReturnsAsync(imagesResponse);

            _documentsGetterServiceMock
                .Setup(x => x.GetDocuments(It.Is<SearchSortRequestDTO>(r => r.IsPublic == true && r.Take == 6 && r.Offset == 0)))
                .ReturnsAsync(documentsResponse);

            // Act
            var result = await _service.GetPrincipalInformation();

            // Assert (FluentAssertions)
            result.Should().NotBeNull();
            result.News.Should().BeEquivalentTo(newsResponse.Data);
            result.Images.Should().BeEquivalentTo(imagesResponse.Data);
            result.Documents.Should().BeEquivalentTo(documentsResponse.Data);

            // Verificar que se invocaron los servicios secundarios exactamente una vez
            _newsGetterServiceMock.Verify(x => x.GetNews(It.IsAny<SearchSortRequestDTO>()), Times.Once);
            _imagesGetterServiceMock.Verify(x => x.GetImages(It.IsAny<SearchSortRequestDTO>()), Times.Once);
            _documentsGetterServiceMock.Verify(x => x.GetDocuments(It.IsAny<SearchSortRequestDTO>()), Times.Once);
        }

        [Fact]
        public async Task GetPrincipalAdminInformation_Should_Return_Admin_Data_Successfully()
        {
            // Arrange
            var newsResponse = _fixture.Create<PageResponseDTO<NewMinimalResponseDTO>>();
            var imagesResponse = _fixture.Create<PageResponseDTO<ImageMinimalResponseDTO>>();
            var documentsResponse = _fixture.Create<PageResponseDTO<DocumentMinimalResponseDTO>>();

            // Validamos que el request interno configure obligatoriamente IsPublic = false
            _newsGetterServiceMock
                .Setup(x => x.GetNews(It.Is<SearchSortRequestDTO>(r => r.IsPublic == false && r.Take == 6 && r.Offset == 0)))
                .ReturnsAsync(newsResponse);

            _imagesGetterServiceMock
                .Setup(x => x.GetImages(It.Is<SearchSortRequestDTO>(r => r.IsPublic == false && r.Take == 6 && r.Offset == 0)))
                .ReturnsAsync(imagesResponse);

            _documentsGetterServiceMock
                .Setup(x => x.GetDocuments(It.Is<SearchSortRequestDTO>(r => r.IsPublic == false && r.Take == 6 && r.Offset == 0)))
                .ReturnsAsync(documentsResponse);

            // Act
            var result = await _service.GetPrincipalAdminInformation();

            // Assert 
            result.Should().NotBeNull();
            result.News.Should().BeEquivalentTo(newsResponse.Data);
            result.Images.Should().BeEquivalentTo(imagesResponse.Data);
            result.Documents.Should().BeEquivalentTo(documentsResponse.Data);

            // Verificar ejecuciones
            _newsGetterServiceMock.Verify(x => x.GetNews(It.IsAny<SearchSortRequestDTO>()), Times.Once);
            _imagesGetterServiceMock.Verify(x => x.GetImages(It.IsAny<SearchSortRequestDTO>()), Times.Once);
            _documentsGetterServiceMock.Verify(x => x.GetDocuments(It.IsAny<SearchSortRequestDTO>()), Times.Once);
        }
    }
}
