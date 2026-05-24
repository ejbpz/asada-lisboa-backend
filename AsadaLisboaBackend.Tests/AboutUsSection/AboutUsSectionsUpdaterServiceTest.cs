using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;

using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Services.AboutUsSections;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;


namespace AsadaLisboaBackend.Tests.AboutUsSection
{
    public class AboutUsSectionsUpdaterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<ILogger<AboutUsSectionsUpdaterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<IAboutUsSectionsUpdaterRepository> _aboutUsSectionsUpdaterRepositoryMock;

        private readonly AboutUsSectionsUpdaterService _service;

        public AboutUsSectionsUpdaterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors
                .Add(new OmitOnRecursionBehavior());

            _loggerMock =
                new Mock<ILogger<AboutUsSectionsUpdaterService>>();

            _memoryCachesServiceMock =
                new Mock<IMemoryCachesService>();

            _aboutUsSectionsUpdaterRepositoryMock =
                new Mock<IAboutUsSectionsUpdaterRepository>();

            _service = new AboutUsSectionsUpdaterService(
                _aboutUsSectionsUpdaterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task UpdateAboutUsSection_Should_Successfully()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Build<AboutUsRequestDTO>()
                .Create();

            var updatedSection = _fixture.Build<Models.AboutUsSection>()
                .With(x => x.Id, id)
                .With(x => x.Order, request.Order)
                .With(x => x.Content, request.Content)
                .With(x => x.SectionType, request.SectionType)
                .Create();

            _aboutUsSectionsUpdaterRepositoryMock
                .Setup(x => x.UpdateAboutUsSection(id, request))
                .ReturnsAsync(updatedSection);

            // Act
            var result =
                await _service.UpdateAboutUsSection(id, request);

            // Assert
            result.Should().NotBeNull();

            result.Id.Should().Be(id);

            result.Order.Should().Be(request.Order);

            result.Content.Should().Be(request.Content);

            result.SectionType.Should().Be(request.SectionType);

            _aboutUsSectionsUpdaterRepositoryMock.Verify(
                x => x.UpdateAboutUsSection(id, request),
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
        public async Task UpdateAboutUsSection_Should_Throw_NotFoundException_When_Repository_Fails()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Create<AboutUsRequestDTO>();

            _aboutUsSectionsUpdaterRepositoryMock
                .Setup(x => x.UpdateAboutUsSection(id, request))
                .ThrowsAsync(new NotFoundException("Error al modificar la sección."));

            // Act
            Func<Task> act = async () =>
                await _service.UpdateAboutUsSection(id, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Error al modificar la sección.");

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
}
