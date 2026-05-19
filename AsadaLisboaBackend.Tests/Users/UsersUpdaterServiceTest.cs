using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Users;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Users
{
    public class UsersUpdaterServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<RoleManager<ApplicationRole>> _roleManagerMock;
        private readonly Mock<IChargesGetterRepository> _chargesGetterRepositoryMock;
        private readonly Mock<ILogger<UsersUpdaterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly UsersUpdaterService _service;

        public UsersUpdaterServiceTests()
        {
            _userManagerMock = CreateUserManagerMock();
            _roleManagerMock = CreateRoleManagerMock();
            _chargesGetterRepositoryMock =
                new Mock<IChargesGetterRepository>();
            _loggerMock =
                new Mock<ILogger<UsersUpdaterService>>();
            _memoryCachesServiceMock =
                new Mock<IMemoryCachesService>();

            _service = new UsersUpdaterService(
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _chargesGetterRepositoryMock.Object,
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

        private Mock<RoleManager<ApplicationRole>> CreateRoleManagerMock()
        {
            var store = new Mock<IRoleStore<ApplicationRole>>();

            return new Mock<RoleManager<ApplicationRole>>(
                store.Object,
                null!,
                null!,
                null!,
                null!);
        }

        [Fact]
        public async Task UpdateUser_Should_Update_User_Successfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var chargeId = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            var user = new ApplicationUser
            {
                Id = userId,
                Email = "test@test.com"
            };

            var charge = new Charge
            {
                Id = chargeId
            };

            var role = new ApplicationRole
            {
                Id = roleId,
                Name = "Admin"
            };

            var request = new UserUpdateRequestDTO
            {
                ChargeId = chargeId,
                RoleId = roleId,
                FirstName = "Eduardo",
                PhoneNumber = "88888888",
                FirstLastName = "Brenes",
                SecondLastName = "Test"
            };

            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _chargesGetterRepositoryMock
                .Setup(x => x.GetCharge(chargeId))
                .ReturnsAsync(charge.ToChargeResponseDTO());

            _roleManagerMock
                .Setup(x => x.FindByIdAsync(roleId.ToString()))
                .ReturnsAsync(role);

            _userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            _userManagerMock
                .Setup(x => x.RemoveFromRoleAsync(user, "User"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(user, role.Name!))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.UpdateUser(userId, request);

            // Assert
            user.FirstName.Should().Be("Eduardo");
            user.PhoneNumber.Should().Be("88888888");
            user.FirstLastName.Should().Be("Brenes");
            user.SecondLastName.Should().Be("Test");
            user.ChargeId.Should().Be(chargeId);

            _userManagerMock.Verify(
                x => x.UpdateAsync(user),
                Times.Once);

            _userManagerMock.Verify(
                x => x.RemoveFromRoleAsync(user, "User"),
                Times.Once);

            _userManagerMock.Verify(
                x => x.AddToRoleAsync(user, "Admin"),
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
        public async Task UpdateUser_Should_Throw_When_User_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UserUpdateRequestDTO();

            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            Func<Task> act = async () =>
                await _service.UpdateUser(userId, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Usuario inexistente.");
        }

        [Fact]
        public async Task UpdateUser_Should_Throw_When_Charge_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UserUpdateRequestDTO
            {
                ChargeId = Guid.NewGuid()
            };

            var user = new ApplicationUser
            {
                Id = userId
            };

            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _chargesGetterRepositoryMock
                .Setup(x => x.GetCharge(request.ChargeId))!
                .ReturnsAsync((ChargeResponseDTO?)null);

            // Act
            Func<Task> act = async () =>
                await _service.UpdateUser(userId, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Cargo seleccionado no encontrado.");
        }

        [Fact]
        public async Task UpdateUser_Should_Throw_When_Role_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var chargeId = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            var request = new UserUpdateRequestDTO
            {
                ChargeId = chargeId,
                RoleId = roleId
            };

            var user = new ApplicationUser
            {
                Id = userId
            };

            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _chargesGetterRepositoryMock
                .Setup(x => x.GetCharge(chargeId))
                .ReturnsAsync(new ChargeResponseDTO
                {
                    Id = chargeId
                });

            _roleManagerMock
                .Setup(x => x.FindByIdAsync(roleId.ToString()))
                .ReturnsAsync((ApplicationRole?)null);

            // Act
            Func<Task> act = async () =>
                await _service.UpdateUser(userId, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Rol seleccionado no encontrado.");
        }

        [Fact]
        public async Task UpdateUser_Should_Throw_IdentityErrorException_When_Update_Fails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var chargeId = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            var request = new UserUpdateRequestDTO
            {
                ChargeId = chargeId,
                RoleId = roleId
            };

            var user = new ApplicationUser
            {
                Id = userId
            };

            var role = new ApplicationRole
            {
                Id = roleId,
                Name = "Admin"
            };

            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _chargesGetterRepositoryMock
                .Setup(x => x.GetCharge(chargeId))
                .ReturnsAsync(new ChargeResponseDTO
                {
                    Id = chargeId
                });

            _roleManagerMock
                .Setup(x => x.FindByIdAsync(roleId.ToString()))
                .ReturnsAsync(role);

            _userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "DuplicateUserName",
                            Description = "Username already exists"
                        }));

            // Act
            Func<Task> act = async () =>
                await _service.UpdateUser(userId, request);

            // Assert
            var exception = await act.Should()
                .ThrowAsync<IdentityErrorException>();

            exception.Which.Errors
                .First().Code
                .Should().Be("DuplicateUserName");
        }

        [Fact]
        public async Task UpdateUser_Should_Not_Remove_Role_When_Role_Is_The_Same()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var chargeId = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            var user = new ApplicationUser
            {
                Id = userId
            };

            var role = new ApplicationRole
            {
                Id = roleId,
                Name = "Admin"
            };

            var request = new UserUpdateRequestDTO
            {
                ChargeId = chargeId,
                RoleId = roleId
            };

            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _chargesGetterRepositoryMock
                .Setup(x => x.GetCharge(chargeId))
                .ReturnsAsync(new ChargeResponseDTO
                {
                    Id = chargeId
                });

            _roleManagerMock
                .Setup(x => x.FindByIdAsync(roleId.ToString()))
                .ReturnsAsync(role);

            _userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin" });

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(user, "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.UpdateUser(userId, request);

            // Assert
            _userManagerMock.Verify(
                x => x.RemoveFromRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
                Times.Never);
        }
    }
}
