using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.AboutUsSections;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Tests.AboutUsSection;

public class AboutUsSectionsDeleterServiceTest
{
    private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
    private readonly Mock<IAboutUsSectionsDeleterRepository> _aboutUsSectionsDeleterRepositoryMock;

    private readonly AboutUsSectionsDeleterService _service;


    public AboutUsSectionsDeleterServiceTest()
    {
        var fixture = new Fixture();

        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(x => fixture.Behaviors.Remove(x));

        fixture.Behaviors.Add(new OmitOnRecursionBehavior());


        var loggerMock = new Mock<ILogger<AboutUsSectionsDeleterService>>();
            
        _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
        _aboutUsSectionsDeleterRepositoryMock = new Mock<IAboutUsSectionsDeleterRepository>();

        _service = new AboutUsSectionsDeleterService(
                
            _aboutUsSectionsDeleterRepositoryMock.Object,
            loggerMock.Object,
            _memoryCachesServiceMock.Object
        );

    }



    [Fact]
    public async Task DeleteAboutUsSections_Should_Delete_Successfully()
    {
        // Arrange
        var id = Guid.NewGuid();

        _aboutUsSectionsDeleterRepositoryMock
            .Setup(x => x.DeleteAboutUsSection(id))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAboutUsSection(id);

        // Assert
        _aboutUsSectionsDeleterRepositoryMock.Verify(
            x => x.DeleteAboutUsSection(id),
            Times.Once);

        _memoryCachesServiceMock.Verify(
            x => x.RemoveById(
                Constants.CACHE_ABOUT_US,
                id),
            Times.Once);

        _memoryCachesServiceMock.Verify(
            x => x.ChangeVersion(
                Constants.CACHE_ABOUT_US),
            Times.Once);

    }

    [Fact]
    public async Task DeleteAboutUsSection_Should_Not_Update_Cache_When_Exception_Occurs()
    {
        // Arrange
        var id = Guid.NewGuid();

        _aboutUsSectionsDeleterRepositoryMock
            .Setup(x => x.DeleteAboutUsSection(id))
            .ThrowsAsync(new Exception("Delete error"));

        // Act
        Func<Task> act = async () =>
            await _service.DeleteAboutUsSection(id);

        await act.Should()
            .ThrowAsync<Exception>();

        // Assert
        _memoryCachesServiceMock.Verify(
            x => x.RemoveById(
                It.IsAny<string>(),
                It.IsAny<Guid>()),
            Times.Never);

        _memoryCachesServiceMock.Verify(
            x => x.ChangeVersion(
                It.IsAny<string>()),
            Times.Never);
    }
}