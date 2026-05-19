using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Roles;
using AsadaLisboaBackend.Models.DTOs.Role;
using AsadaLisboaBackend.RepositoryContracts.Roles;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Roles
{
    public class RolesGetterServiceTest
    {
        private readonly Mock<ILogger<RolesGetterService>> _loggerMock;

        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly Mock<IRolesGetterRepository> _rolesGetterRepositoryMock;

        private readonly RolesGetterService _service;

        public RolesGetterServiceTest()
        {
            _loggerMock =
                new Mock<ILogger<RolesGetterService>>();

            _memoryCachesServiceMock =
                new Mock<IMemoryCachesService>();

            _rolesGetterRepositoryMock =
                new Mock<IRolesGetterRepository>();

            _service = new RolesGetterService(
                _rolesGetterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object);
        }

        [Fact]
        public async Task GetRoles_Should_Return_Roles_From_Cache()
        {
            // Arrange
            var roles = new List<RoleResponseDTO>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "User"
                }
            };

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<List<RoleResponseDTO>>(
                    Constants.CACHE_ROLES,
                    "",
                    It.IsAny<Func<Task<List<RoleResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(roles);

            // Act
            var result = await _service.GetRoles();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(roles);
        }

        [Fact]
        public async Task GetRoles_Should_Call_Cache_Service_Correctly()
        {
            // Arrange
            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<List<RoleResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<Func<Task<List<RoleResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(new List<RoleResponseDTO>());

            // Act
            await _service.GetRoles();

            // Assert
            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCacheList<List<RoleResponseDTO>>(
                    Constants.CACHE_ROLES,
                    "",
                    It.IsAny<Func<Task<List<RoleResponseDTO>>>>(),
                    It.Is<TimeSpan>(t => t == TimeSpan.FromMinutes(5))),
                Times.Once);
        }

        [Fact]
        public async Task GetRoles_Should_Use_Repository_Inside_Cache_Delegate()
        {
            // Arrange
            var roles = new List<RoleResponseDTO>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin"
                }
            };

            _rolesGetterRepositoryMock
                .Setup(x => x.GetRoles())
                .ReturnsAsync(roles);

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<List<RoleResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<Func<Task<List<RoleResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .Returns<string, object, Func<Task<List<RoleResponseDTO>>>, TimeSpan>(
                    async (_, _, create, _) => await create());

            // Act
            var result = await _service.GetRoles();

            // Assert
            result.Should().HaveCount(1);

            _rolesGetterRepositoryMock.Verify(
                x => x.GetRoles(),
                Times.Once);
        }

        [Fact]
        public async Task GetRoles_Should_Propagate_Exception_When_Repository_Fails()
        {
            // Arrange
            _rolesGetterRepositoryMock
                .Setup(x => x.GetRoles())
                .ThrowsAsync(new Exception("Database error"));

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<List<RoleResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<Func<Task<List<RoleResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .Returns<string, object, Func<Task<List<RoleResponseDTO>>>, TimeSpan>(
                    async (_, _, create, _) => await create());

            // Act
            Func<Task> act = async () =>
                await _service.GetRoles();

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database error");
        }
    }
}
