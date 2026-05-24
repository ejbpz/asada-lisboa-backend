using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Tests.Editors
{
    public class EditorsUpdaterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IFileSystemsManager> _fileSystemsManagerMock;
        private readonly Mock<ILogger<EditorsUpdaterService>> _loggerMock;

        private readonly EditorsUpdaterService _service;

        public EditorsUpdaterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fileSystemsManagerMock = new Mock<IFileSystemsManager>();

            _loggerMock = new Mock<ILogger<EditorsUpdaterService>>();

            _service = new EditorsUpdaterService(
                _fileSystemsManagerMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task ChangeHtmlImagesFolder_Should_Move_Images_Successfully()
        {
            // Arrange
            var html = @"
                <html>
                    <body>
                        <img src='https://server/temp/image1.jpg' />
                        <img src='https://server/temp/image2.png' />
                    </body>
                </html>";

            _fileSystemsManagerMock
                .Setup(x => x.MoveAsync(
                    It.IsAny<string>(),
                    "temp",
                    "noticias"))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.ChangeHtmlImagesFolder(html);

            // Assert
            result.Should().Contain($"/noticias/image1.jpg");
            result.Should().Contain($"/noticias/image2.png");

            _fileSystemsManagerMock.Verify(
                x => x.MoveAsync(
                    "image1.jpg",
                    "temp",
                    "noticias"),
                Times.Once);

            _fileSystemsManagerMock.Verify(
                x => x.MoveAsync(
                    "image2.png",
                    "temp",
                    "noticias"),
                Times.Once);
        }

        [Fact]
        public async Task ChangeHtmlImagesFolder_Should_Return_Same_Html_When_No_Temp_Images_Exist()
        {
            // Arrange
            var html = @"
                <html>
                    <body>
                        <p>No temp images</p>
                    </body>
                </html>";

            // Act
            var result = await _service.ChangeHtmlImagesFolder(html);

            // Assert
            result.Should().Be(html);

            _fileSystemsManagerMock.Verify(
                x => x.MoveAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task ChangeHtmlImagesFolder_Should_Ignore_Invalid_Src()
        {
            // Arrange
            var html = @"
                <html>
                    <body>
                        <img src='' />
                        <img />
                    </body>
                </html>";

            // Act
            var result = await _service.ChangeHtmlImagesFolder(html);

            // Assert
            result.Should().NotBeNull();

            _fileSystemsManagerMock.Verify(
                x => x.MoveAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task ChangeHtmlImagesFolder_Should_Call_Logger_When_Success()
        {
            // Arrange
            var html = @"
                <html>
                    <body>
                        <img src='https://server/temp/image1.jpg' />
                    </body>
                </html>";

            _fileSystemsManagerMock
                .Setup(x => x.MoveAsync(
                    It.IsAny<string>(),
                    "temp",
                    "noticias"))
                .Returns(Task.CompletedTask);

            // Act
            await _service.ChangeHtmlImagesFolder(html);

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
        public async Task ChangeHtmlImagesFolder_Should_Replace_Image_Urls_Correctly()
        {
            // Arrange
            var html = @"
                <html>
                    <body>
                        <img src='https://server/temp/photo.jpg' />
                    </body>
                </html>";

            _fileSystemsManagerMock
                .Setup(x => x.MoveAsync(
                    "photo.jpg",
                    "temp",
                    "noticias"))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.ChangeHtmlImagesFolder(html);

            // Assert
            result.Should()
                .Contain($"{Constants.DOMAIN_HOST}/noticias/photo.jpg");
        }
    }
}