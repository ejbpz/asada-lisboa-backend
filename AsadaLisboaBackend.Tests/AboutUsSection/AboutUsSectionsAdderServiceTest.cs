using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;
using  AsadaLisboaBackend.Services.AboutUsSections;

namespace AsadaLisboaBackend.Tests.AboutUsSection
{
    public class AboutUsSectionsAdderServiceTest
    {

        private readonly Fixture _fixture;

        private readonly AboutUsSectionsAdderService _service;
       // private readonly Mock<ILogger<AboutUsSectionsAdderService>> _loggerMock;
        private readonly Mock<ILogger<AboutUsSectionsAdderService>> _loggerMock;
        private readonly Mock<IAboutUsSectionsAdderRepository> _aboutUsSectionsAdderRepositoryMock;
        private readonly Mock<IAboutUsSectionsAdderService> _aboutUsSectionsAdderServiceMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        public  AboutUsSectionsAdderServiceTest()
        {

            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());


            _loggerMock = new Mock<ILogger<AboutUsSectionsAdderService>>();
            _aboutUsSectionsAdderRepositoryMock = new Mock<IAboutUsSectionsAdderRepository>();

            _aboutUsSectionsAdderServiceMock = new Mock<IAboutUsSectionsAdderService>();
            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();

            _service = new AboutUsSectionsAdderService(

                _aboutUsSectionsAdderRepositoryMock.Object,

                _loggerMock.Object,                

                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateNew_Should_Create_New_Successfully()
        {

            // Arrange
            var request = _fixture.Build<AboutUsRequestDTO>()
                .Create();

            var createdSection = _fixture.Build<Models.AboutUsSection>()
                
                .With(x => x.Order, request.Order)
                .With(x => x.Content, request.Content)
                .With(x => x.SectionType, request.SectionType)
                .Create();

            _aboutUsSectionsAdderRepositoryMock
                .Setup(x => x.CreateAboutUsSection(
                    It.IsAny<Models.AboutUsSection>()))
                .ReturnsAsync(createdSection);

            // Act
            var result =
                await _service.CreateAboutUsSection(request);

            // Assert
            result.Should().NotBeNull();

            result.Order.Should().Be(request.Order);

            result.Content.Should().Be(request.Content);

            result.SectionType.Should().Be(request.SectionType);

            _aboutUsSectionsAdderRepositoryMock.Verify(
                x => x.CreateAboutUsSection(
                    It.Is<Models.AboutUsSection>(s =>
                        s.Order == request.Order &&
                        s.Content == request.Content &&
                        s.SectionType == request.SectionType
                    )),
                Times.Once);

            _memoryCachesServiceMock.Verify(
              x => x.ChangeVersion(Constants.CACHE_ABOUT_US),
              Times.Once);
        }


        [Fact]
        public async Task CreateAboutUsSection_Should_Throw_When_Repository_Fails()
        {
            // Arrange
            var request = _fixture.Create<AboutUsRequestDTO>();

            _aboutUsSectionsAdderRepositoryMock
                .Setup(x => x.CreateAboutUsSection(
                    It.IsAny<Models.AboutUsSection>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () =>
                await _service.CreateAboutUsSection(request);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database error");

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAboutUsSection_Should_Call_Logger()
        {
            // Arrange
            var request = _fixture.Create<AboutUsRequestDTO>();

            var createdSection = _fixture.Create<Models.AboutUsSection>();

            _aboutUsSectionsAdderRepositoryMock
                .Setup(x => x.CreateAboutUsSection(
                    It.IsAny<Models.AboutUsSection>()))
                .ReturnsAsync(createdSection);

            // Act
            await _service.CreateAboutUsSection(request);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAboutUsSection_Should_Update_Cache()
        {
            // Arrange
            var request = _fixture.Create<AboutUsRequestDTO>();

            var createdSection = _fixture.Create<Models.AboutUsSection>();

            _aboutUsSectionsAdderRepositoryMock
                .Setup(x => x.CreateAboutUsSection(
                    It.IsAny<Models.AboutUsSection>()))
                .ReturnsAsync(createdSection);

            // Act
            await _service.CreateAboutUsSection(request);

            // Assert
            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_ABOUT_US),
                Times.Once);
        }


    }
}
