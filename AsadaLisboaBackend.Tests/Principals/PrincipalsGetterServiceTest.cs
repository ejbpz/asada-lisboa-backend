using Moq;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Image; 
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Services.Principals;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Documents;

namespace AsadaLisboaBackend.Tests.Principals;

public class PrincipalsGetterServiceTest
{
    private readonly Fixture _fixture;
    private readonly PrincipalsGetterService _service;
    private readonly Mock<INewsGetterService> _newsGetterServiceMock;
    private readonly Mock<IImagesGetterService> _imagesGetterServiceMock;
    private readonly Mock<IDocumentsGetterService> _documentsGetterServiceMock;

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
        var loggerMock = new Mock<ILogger<PrincipalsGetterService>>();

        _service = new PrincipalsGetterService(
            _newsGetterServiceMock.Object,
            _imagesGetterServiceMock.Object,
            _documentsGetterServiceMock.Object,
            loggerMock.Object
        );
    }

    [Fact]
    public async Task GetPrincipalInformation_Should_Return_Public_Data_Successfully()
    {
        // Arrange
        var newsResponse = _fixture.Create<PageResponseDto<NewMinimalResponseDto>>();
        var imagesResponse = _fixture.Create<PageResponseDto<ImageMinimalResponseDto>>();
        var documentsResponse = _fixture.Create<PageResponseDto<DocumentMinimalResponseDto>>();

        // Validamos que el request interno configure obligatoriamente IsPublic = true
        _newsGetterServiceMock
            .Setup(x => x.GetNews(It.Is<SearchSortRequestDto>(r => r.IsPublic == true && r.Take == 6 && r.Offset == 0)))
            .ReturnsAsync(newsResponse);

        _imagesGetterServiceMock
            .Setup(x => x.GetImages(It.Is<SearchSortRequestDto>(r => r.IsPublic == true && r.Take == 6 && r.Offset == 0)))
            .ReturnsAsync(imagesResponse);

        _documentsGetterServiceMock
            .Setup(x => x.GetDocuments(It.Is<SearchSortRequestDto>(r => r.IsPublic == true && r.Take == 6 && r.Offset == 0)))
            .ReturnsAsync(documentsResponse);

        // Act
        var result = await _service.GetPrincipalInformation();

        // Assert (FluentAssertions)
        result.Should().NotBeNull();
        result.News.Should().BeEquivalentTo(newsResponse.Data);
        result.Images.Should().BeEquivalentTo(imagesResponse.Data);
        result.Documents.Should().BeEquivalentTo(documentsResponse.Data);

        // Verificar que se invocaron los servicios secundarios exactamente una vez
        _newsGetterServiceMock.Verify(x => x.GetNews(It.IsAny<SearchSortRequestDto>()), Times.Once);
        _imagesGetterServiceMock.Verify(x => x.GetImages(It.IsAny<SearchSortRequestDto>()), Times.Once);
        _documentsGetterServiceMock.Verify(x => x.GetDocuments(It.IsAny<SearchSortRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task GetPrincipalAdminInformation_Should_Return_Admin_Data_Successfully()
    {
        // Arrange
        var newsResponse = _fixture.Create<PageResponseDto<NewMinimalResponseDto>>();
        var imagesResponse = _fixture.Create<PageResponseDto<ImageMinimalResponseDto>>();
        var documentsResponse = _fixture.Create<PageResponseDto<DocumentMinimalResponseDto>>();

        // Validamos que el request interno configure obligatoriamente IsPublic = false
        _newsGetterServiceMock
            .Setup(x => x.GetNews(It.Is<SearchSortRequestDto>(r => r.IsPublic == false && r.Take == 6 && r.Offset == 0)))
            .ReturnsAsync(newsResponse);

        _imagesGetterServiceMock
            .Setup(x => x.GetImages(It.Is<SearchSortRequestDto>(r => r.IsPublic == false && r.Take == 6 && r.Offset == 0)))
            .ReturnsAsync(imagesResponse);

        _documentsGetterServiceMock
            .Setup(x => x.GetDocuments(It.Is<SearchSortRequestDto>(r => r.IsPublic == false && r.Take == 6 && r.Offset == 0)))
            .ReturnsAsync(documentsResponse);

        // Act
        var result = await _service.GetPrincipalAdminInformation();

        // Assert 
        result.Should().NotBeNull();
        result.News.Should().BeEquivalentTo(newsResponse.Data);
        result.Images.Should().BeEquivalentTo(imagesResponse.Data);
        result.Documents.Should().BeEquivalentTo(documentsResponse.Data);

        // Verificar ejecuciones
        _newsGetterServiceMock.Verify(x => x.GetNews(It.IsAny<SearchSortRequestDto>()), Times.Once);
        _imagesGetterServiceMock.Verify(x => x.GetImages(It.IsAny<SearchSortRequestDto>()), Times.Once);
        _documentsGetterServiceMock.Verify(x => x.GetDocuments(It.IsAny<SearchSortRequestDto>()), Times.Once);
    }
}