using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Users;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Users
{
    public class UsersDeleterServiceTest
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<ILogger<UsersDeleterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly UsersDeleterService _service;

        public UsersDeleterServiceTest()
        {
            _userManagerMock = CreateUserManagerMock();
            _loggerMock =
                new Mock<ILogger<UsersDeleterService>>();
            _memoryCachesServiceMock =
                new Mock<IMemoryCachesService>();

            _service = new UsersDeleterService(
                _userManagerMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object);
        }

        private Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            return new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);
        }

        [Fact]
        public async Task DeleteUser_Should_Delete_User_And_Clear_Cache()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = userId,
                Email = "test@test.com"
            };

            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.DeleteUser(userId);

            // Assert
            _userManagerMock.Verify(
                x => x.FindByIdAsync(userId.ToString()),
                Times.Once);

            _userManagerMock.Verify(
                x => x.DeleteAsync(user),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(
                    Constants.CACHE_USERS,
                    user.Id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(
                    Constants.CACHE_USERS),
                Times.Once);
        }

        [Fact]
        public async Task DeleteUser_Should_Throw_When_User_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            Func<Task> act = async () =>
                await _service.DeleteUser(userId);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Usuario inexistente.");

            _userManagerMock.Verify(
                x => x.DeleteAsync(It.IsAny<ApplicationUser>()),
                Times.Never);

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
        public async Task DeleteUser_Should_Clear_Cache_After_Delete()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = userId
            };

            var sequence = new MockSequence();

            _userManagerMock
                .InSequence(sequence)
                .Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock
                .InSequence(sequence)
                .Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            _memoryCachesServiceMock
                .InSequence(sequence)
                .Setup(x => x.RemoveById(
                    Constants.CACHE_USERS,
                    user.Id));

            _memoryCachesServiceMock
                .InSequence(sequence)
                .Setup(x => x.ChangeVersion(
                    Constants.CACHE_USERS));

            // Act
            await _service.DeleteUser(userId);

            // Assert
            _userManagerMock.VerifyAll();
            _memoryCachesServiceMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteUser_Should_Not_Clear_Cache_When_Delete_Fails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = userId
            };

            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "Delete failed"
                        }));

            // Act
            Func<Task> act = async () =>
                await _service.DeleteUser(userId);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Error al eliminar el usuario.");

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
