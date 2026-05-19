using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Categories;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Tests.Categories
{
    public class CategoriesDeleterServiceTest
    {
        private readonly Mock<ILogger<CategoriesDeleterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<ICategoriesDeleterRepository> _categoriesDeleterRepositoryMock;

        private readonly CategoriesDeleterService _service;

        public CategoriesDeleterServiceTest()
        {
            _loggerMock = new Mock<ILogger<CategoriesDeleterService>>();

            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();

            _categoriesDeleterRepositoryMock =
                new Mock<ICategoriesDeleterRepository>();

            _service = new CategoriesDeleterService(
                _categoriesDeleterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task DeleteCategory_Should_Delete_Category_And_Clear_Cache()
        {
            // Arrange
            var id = Guid.NewGuid();

            _categoriesDeleterRepositoryMock
                .Setup(x => x.DeleteCategory(id))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteCategory(id);

            // Assert
            _categoriesDeleterRepositoryMock.Verify(
                x => x.DeleteCategory(id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(
                    Constants.CACHE_CATEGORIES,
                    id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(
                    Constants.CACHE_CATEGORIES),
                Times.Once);
        }

        [Fact]
        public async Task DeleteCategory_Should_Propagate_Exception_When_Delete_Fails()
        {
            // Arrange
            var id = Guid.NewGuid();

            _categoriesDeleterRepositoryMock
                .Setup(x => x.DeleteCategory(id))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () =>
                await _service.DeleteCategory(id);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database error");

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

        [Fact]
        public async Task DeleteCategory_Should_Clear_Cache_After_Delete()
        {
            // Arrange
            var id = Guid.NewGuid();

            var sequence = new MockSequence();

            _categoriesDeleterRepositoryMock
                .InSequence(sequence)
                .Setup(x => x.DeleteCategory(id))
                .Returns(Task.CompletedTask);

            _memoryCachesServiceMock
                .InSequence(sequence)
                .Setup(x => x.RemoveById(
                    Constants.CACHE_CATEGORIES,
                    id));

            _memoryCachesServiceMock
                .InSequence(sequence)
                .Setup(x => x.ChangeVersion(
                    Constants.CACHE_CATEGORIES));

            // Act
            await _service.DeleteCategory(id);

            // Assert
            _categoriesDeleterRepositoryMock.VerifyAll();
            _memoryCachesServiceMock.VerifyAll();
        }
    }
}
