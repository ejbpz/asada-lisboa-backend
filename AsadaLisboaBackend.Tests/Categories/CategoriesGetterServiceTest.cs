using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Categories;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.RepositoryContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Categories
{
    public class CategoriesGetterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<ICategoriesAdderRepository> _categoriesAdderRepositoryMock;
        private readonly Mock<ICategoriesGetterRepository> _categoriesGetterRepositoryMock;

        private readonly CategoriesGetterService _service;

        public CategoriesGetterServiceTest()
        {
            _fixture = new Fixture();

            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
            _categoriesAdderRepositoryMock = new Mock<ICategoriesAdderRepository>();
            _categoriesGetterRepositoryMock = new Mock<ICategoriesGetterRepository>();

            _service = new CategoriesGetterService(
                _categoriesGetterRepositoryMock.Object,
                _categoriesAdderRepositoryMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task GetCategories_Should_Return_Cached_Categories()
        {
            // Arrange
            var categories = _fixture.Create<List<CategoryResponseDTO>>();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache<List<CategoryResponseDTO>>(
                    Constants.CACHE_CATEGORIES,
                    It.IsAny<Func<Task<List<CategoryResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _service.GetCategories();

            // Assert
            result.Should().BeEquivalentTo(categories);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCache<List<CategoryResponseDTO>>(
                    Constants.CACHE_CATEGORIES,
                    It.IsAny<Func<Task<List<CategoryResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Fact]
        public async Task SearchCategories_Should_Normalize_Search_Text()
        {
            // Arrange
            var search = "   Tecnologia   ";
            var normalized = "tecnologia";

            var categories = _fixture.Create<List<CategoryResponseDTO>>();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<List<CategoryResponseDTO>>(
                    Constants.CACHE_CATEGORIES,
                    normalized,
                    It.IsAny<Func<Task<List<CategoryResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _service.SearchCategories(search);

            // Assert
            result.Should().BeEquivalentTo(categories);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCacheList<List<CategoryResponseDTO>>(
                    Constants.CACHE_CATEGORIES,
                    normalized,
                    It.IsAny<Func<Task<List<CategoryResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Fact]
        public async Task ToCreateCategories_Should_Return_Existing_Ids()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var request = new List<CategoryRequestDTO>
            {
                new()
                {
                    Id = categoryId,
                    Name = "Tecnologia"
                }
            };

            _categoriesGetterRepositoryMock
                .Setup(x => x.NoRepeatNames(It.IsAny<List<string>>()))
                .ReturnsAsync(new List<CategoryResponseDTO>());

            _categoriesGetterRepositoryMock
                .Setup(x => x.ToCreateCategories(
                    It.IsAny<List<CategoryResponseDTO>>(),
                    It.IsAny<List<string>>()))
                .Returns(new List<Category>());

            // Act
            var result = await _service.ToCreateCategories(request);

            // Assert
            result.Should().ContainSingle();
            result.First().Id.Should().Be(categoryId);
        }

        [Fact]
        public async Task ToCreateCategories_Should_Create_New_Categories()
        {
            // Arrange
            var request = new List<CategoryRequestDTO>
            {
                new()
                {
                    Name = "Tecnologia"
                }
            };

            var toCreate = new List<Category>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Tecnologia"
                }
            };

            _categoriesGetterRepositoryMock
                .Setup(x => x.NoRepeatNames(It.IsAny<List<string>>()))
                .ReturnsAsync(new List<CategoryResponseDTO>());

            _categoriesGetterRepositoryMock
                .Setup(x => x.ToCreateCategories(
                    It.IsAny<List<CategoryResponseDTO>>(),
                    It.IsAny<List<string>>()))
                .Returns(toCreate);

            // Act
            var result = await _service.ToCreateCategories(request);

            // Assert
            result.Should().ContainSingle(x =>
                x.Id == toCreate[0].Id);

            _categoriesAdderRepositoryMock.Verify(
                x => x.AddCategories(toCreate),
                Times.Once);
        }

        [Fact]
        public async Task ToCreateCategories_Should_Not_Create_When_Name_Already_Exists()
        {
            // Arrange
            var existingId = Guid.NewGuid();

            var request = new List<CategoryRequestDTO>
            {
                new()
                {
                    Name = "Tecnologia"
                }
            };

            var existing = new List<CategoryResponseDTO>
            {
                new()
                {
                    Id = existingId,
                    Name = "Tecnologia"
                }
            };

            _categoriesGetterRepositoryMock
                .Setup(x => x.NoRepeatNames(It.IsAny<List<string>>()))
                .ReturnsAsync(existing);

            _categoriesGetterRepositoryMock
                .Setup(x => x.ToCreateCategories(
                    It.IsAny<List<CategoryResponseDTO>>(),
                    It.IsAny<List<string>>()))
                .Returns(new List<Category>());

            // Act
            var result = await _service.ToCreateCategories(request);

            // Assert
            result.Should().ContainSingle(x =>
                x.Id == existingId);

            _categoriesAdderRepositoryMock.Verify(
                x => x.AddCategories(It.IsAny<List<Category>>()),
                Times.Never);
        }

        [Fact]
        public async Task ToCreateCategories_Should_Combine_All_Category_Sources()
        {
            // Arrange
            var existingId = Guid.NewGuid();
            var byId = Guid.NewGuid();
            var newId = Guid.NewGuid();

            var request = new List<CategoryRequestDTO>
            {
                new()
                {
                    Id = byId,
                    Name = "Backend"
                },

                new()
                {
                    Name = "Frontend"
                },

                new()
                {
                    Name = "DevOps"
                }
            };

            var existingByName = new List<CategoryResponseDTO>
            {
                new()
                {
                    Id = existingId,
                    Name = "Frontend"
                }
            };

            var toCreate = new List<Category>
            {
                new()
                {
                    Id = newId,
                    Name = "DevOps"
                }
            };

            _categoriesGetterRepositoryMock
                .Setup(x => x.NoRepeatNames(It.IsAny<List<string>>()))
                .ReturnsAsync(existingByName);

            _categoriesGetterRepositoryMock
                .Setup(x => x.ToCreateCategories(
                    It.IsAny<List<CategoryResponseDTO>>(),
                    It.IsAny<List<string>>()))
                .Returns(toCreate);

            // Act
            var result = await _service.ToCreateCategories(request);

            // Assert
            result.Should().HaveCount(3);
            result.Should().Contain(x => x.Id == byId);
            result.Should().Contain(x => x.Id == existingId);
            result.Should().Contain(x => x.Id == newId);

            _categoriesAdderRepositoryMock.Verify(
                x => x.AddCategories(toCreate),
                Times.Once);
        }

        [Fact]
        public async Task ToCreateCategories_Should_Remove_Duplicate_Ids()
        {
            // Arrange
            var duplicatedId = Guid.NewGuid();

            var request = new List<CategoryRequestDTO>
            {
                new()
                {
                    Id = duplicatedId,
                    Name = "Backend"
                },

                new()
                {
                    Name = "Backend"
                }
            };

            var existingByName = new List<CategoryResponseDTO>
            {
                new()
                {
                    Id = duplicatedId,
                    Name = "Backend"
                }
            };

            _categoriesGetterRepositoryMock
                .Setup(x => x.NoRepeatNames(It.IsAny<List<string>>()))
                .ReturnsAsync(existingByName);

            _categoriesGetterRepositoryMock
                .Setup(x => x.ToCreateCategories(
                    It.IsAny<List<CategoryResponseDTO>>(),
                    It.IsAny<List<string>>()))
                .Returns(new List<Category>());

            // Act
            var result = await _service.ToCreateCategories(request);

            // Assert
            result.Should().HaveCount(1);
            result.First().Id.Should().Be(duplicatedId);
        }

        [Fact]
        public async Task ToCreateCategories_Should_Normalize_Category_Names()
        {
            // Arrange
            var request = new List<CategoryRequestDTO>
            {
                new()
                {
                    Name = "   Tecnologia   "
                }
            };

            List<string>? receivedNames = null;

            _categoriesGetterRepositoryMock
                .Setup(x => x.NoRepeatNames(It.IsAny<List<string>>()))
                .Callback<List<string>>(names =>
                {
                    receivedNames = names;
                })
                .ReturnsAsync(new List<CategoryResponseDTO>());

            _categoriesGetterRepositoryMock
                .Setup(x => x.ToCreateCategories(
                    It.IsAny<List<CategoryResponseDTO>>(),
                    It.IsAny<List<string>>()))
                .Returns(new List<Category>());

            // Act
            await _service.ToCreateCategories(request);

            // Assert
            receivedNames.Should().ContainSingle();
            receivedNames!.First().Should().Be("tecnologia");
        }
    }
}
