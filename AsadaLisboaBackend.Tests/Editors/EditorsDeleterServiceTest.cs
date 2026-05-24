using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Services.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Tests.Editors
{
    public class EditorsDeleterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IFileSystemsManager> _fileSystemsManagerMock;
        private readonly Mock<ILogger<EditorsDeleterService>> _loggerMock;

        private readonly EditorsDeleterService _service;

        public EditorsDeleterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fileSystemsManagerMock = new Mock<IFileSystemsManager>();

            _loggerMock = new Mock<ILogger<EditorsDeleterService>>();

            _service = new EditorsDeleterService(
                _fileSystemsManagerMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task DeletePrincipalImage_Should_Delete_Image_Successfully()
        {
            // Arrange
            var fileName = "image.jpg";

            _fileSystemsManagerMock
                .Setup(x => x.DeleteAsync(fileName, "noticias"))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeletePrincipalImage(fileName);

            // Assert
            _fileSystemsManagerMock.Verify(
                x => x.DeleteAsync(fileName, "noticias"),
                Times.Once);

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
        public async Task DeleteContentImages_Should_Delete_All_Content_Images()
        {
            // Arrange
            var html = @"
                <html>
                    <body>
                        <img src='/noticias/image1.jpg' />
                        <img src='/noticias/image2.png' />
                    </body>
                </html>";

            _fileSystemsManagerMock
                .Setup(x => x.DeleteAsync(It.IsAny<string>(), "noticias"))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteContentImages(html);

            // Assert
            _fileSystemsManagerMock.Verify(
                x => x.DeleteAsync("image1.jpg", "noticias"),
                Times.Once);

            _fileSystemsManagerMock.Verify(
                x => x.DeleteAsync("image2.png", "noticias"),
                Times.Once);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeast(2));
        }

        [Fact]
        public async Task DeleteContentImages_Should_Not_Delete_When_No_Images_Exist()
        {
            // Arrange
            var html = @"
                <html>
                    <body>
                        <p>No images here</p>
                    </body>
                </html>";

            // Act
            await _service.DeleteContentImages(html);

            // Assert
            _fileSystemsManagerMock.Verify(
                x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteUnusedImages_Should_Delete_Unused_Images()
        {
            // Arrange
            var oldHtml = @"
                <html>
                    <body>
                        <img src='/noticias/image1.jpg' />
                        <img src='/noticias/image2.jpg' />
                    </body>
                </html>";

            var newHtml = @"
                <html>
                    <body>
                        <img src='/noticias/image2.jpg' />
                    </body>
                </html>";

            _fileSystemsManagerMock
                .Setup(x => x.DeleteAsync(It.IsAny<string>(), "noticias"))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteUnusedImages(oldHtml, newHtml);

            // Assert
            _fileSystemsManagerMock.Verify(
                x => x.DeleteAsync("image1.jpg", "noticias"),
                Times.Once);

            _fileSystemsManagerMock.Verify(
                x => x.DeleteAsync("image2.jpg", "noticias"),
                Times.Never);
        }

        [Fact]
        public async Task DeleteUnusedImages_Should_Not_Delete_When_All_Images_Are_Used()
        {
            // Arrange
            var oldHtml = @"
                <html>
                    <body>
                        <img src='/noticias/image1.jpg' />
                    </body>
                </html>";

            var newHtml = @"
                <html>
                    <body>
                        <img src='/noticias/image1.jpg' />
                    </body>
                </html>";

            // Act
            await _service.DeleteUnusedImages(oldHtml, newHtml);

            // Assert
            _fileSystemsManagerMock.Verify(
                x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteUnusedImages_Should_Return_When_OldHtml_Has_No_Images()
        {
            // Arrange
            var oldHtml = "<html><body><p>No images</p></body></html>";

            var newHtml = @"
                <html>
                    <body>
                        <img src='/noticias/image1.jpg' />
                    </body>
                </html>";

            // Act
            await _service.DeleteUnusedImages(oldHtml, newHtml);

            // Assert
            _fileSystemsManagerMock.Verify(
                x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }
    }
}