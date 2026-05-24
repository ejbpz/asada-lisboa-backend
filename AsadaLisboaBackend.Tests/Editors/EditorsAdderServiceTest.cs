using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Services.Editors;
using AsadaLisboaBackend.Models.DTOs.Editor;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Tests.Editors
{
    public class EditorsAdderServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IFileSystemsManager> _fileSystemsManagerMock;
        private readonly Mock<ILogger<EditorsAdderService>> _loggerMock;

        private readonly EditorsAdderService _service;

        public EditorsAdderServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fileSystemsManagerMock = new Mock<IFileSystemsManager>();

            _loggerMock = new Mock<ILogger<EditorsAdderService>>();

            _service = new EditorsAdderService(
                _fileSystemsManagerMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CreateTemporalImage_Should_Create_Image_Successfully()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.Length)
                .Returns(100);

            var request = _fixture.Build<EditorRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .Create();

            var expectedUrl = "https://server/temp/image.jpg";

            _fileSystemsManagerMock
                .Setup(x => x.SaveAsync(
                    fileMock.Object,
                    "temp",
                    null))
                .ReturnsAsync(expectedUrl);

            // Act
            var result = await _service.CreateTemporalImage(request);

            // Assert
            result.Should().NotBeNull();

            result.Url.Should().Be(expectedUrl);

            _fileSystemsManagerMock.Verify(
                x => x.SaveAsync(
                    fileMock.Object,
                    "temp",
                    null),
                Times.Once);
        }

        [Fact]
        public async Task CreateTemporalImage_Should_Throw_When_File_Is_Null()
        {
            // Arrange
            var request = _fixture.Build<EditorRequestDTO>()
                .With(x => x.File, (IFormFile?)null)
                .Create();

            // Act
            Func<Task> act = async () => await _service.CreateTemporalImage(request);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("El archivo no puede ser nulo.*");

            _fileSystemsManagerMock.Verify(
                x => x.SaveAsync(
                    It.IsAny<IFormFile>(),
                    It.IsAny<string>(),
                    It.IsAny<string?>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateTemporalImage_Should_Call_Logger_When_Success()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();

            var request = _fixture.Build<EditorRequestDTO>()
                .With(x => x.File, fileMock.Object)
                .Create();

            _fileSystemsManagerMock
                .Setup(x => x.SaveAsync(
                    It.IsAny<IFormFile>(),
                    "temp",
                    null))
                .ReturnsAsync("https://server/temp/image.jpg");

            // Act
            await _service.CreateTemporalImage(request);

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
        public async Task CreateTemporalImage_Should_Call_Logger_When_File_Is_Null()
        {
            // Arrange
            var request = _fixture.Build<EditorRequestDTO>()
                .With(x => x.File, (IFormFile?)null)
                .Create();

            // Act
            Func<Task> act = async () => await _service.CreateTemporalImage(request);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }
    }
}