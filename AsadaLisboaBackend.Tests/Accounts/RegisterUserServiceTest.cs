using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Accounts;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Accounts;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Accounts
{
    public class RegisterUserServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<RoleManager<ApplicationRole>> _roleManagerMock;

        private readonly Mock<ILogger<RegisterUserService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
        private readonly Mock<IChargesGetterRepository> _chargesGetterRepositoryMock;
        private readonly Mock<IVerificationCodeService> _verificationCodeServiceMock;

        private readonly RegisterUserService _service;

        public RegisterUserServiceTest()
        {
            _fixture = new Fixture();

            _userManagerMock = CreateUserManagerMock();
            _roleManagerMock = CreateRoleManagerMock();

            _loggerMock = new Mock<ILogger<RegisterUserService>>();
            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();
            _chargesGetterRepositoryMock = new Mock<IChargesGetterRepository>();
            _verificationCodeServiceMock = new Mock<IVerificationCodeService>();

            _service = new RegisterUserService(
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _verificationCodeServiceMock.Object,
                _chargesGetterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
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
        public async Task RegisterUser_Should_Register_User_Successfully()
        {
            // Arrange
            var request = _fixture.Create<RegisterRequestDTO>();

            var charge = _fixture.Build<Charge>()
                .With(x => x.Id, request.ChargeId)
                .Create();

            var role = _fixture.Build<ApplicationRole>()
                .With(x => x.Id, request.RoleId)
                .With(x => x.Name, "Admin")
                .Create();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _chargesGetterRepositoryMock
                .Setup(x => x.GetCharge(request.ChargeId))
                .ReturnsAsync(charge.ToChargeResponseDTO());

            _roleManagerMock
                .Setup(x => x.FindByIdAsync(request.RoleId.ToString()))
                .ReturnsAsync(role);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    role.Name!))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.RegisterUser(request);

            // Assert

            _userManagerMock.Verify(
                x => x.CreateAsync(
                    It.Is<ApplicationUser>(u =>
                        u.Email == request.Email &&
                        u.UserName == request.Email &&
                        u.FirstName == request.FirstName &&
                        u.ChargeId == request.ChargeId
                    ),
                    request.Password),
                Times.Once);

            _userManagerMock.Verify(
                x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    role.Name!),
                Times.Once);

            _verificationCodeServiceMock.Verify(
                x => x.GenerateCode(request.Email),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_USERS),
                Times.Once);
        }

        [Fact]
        public async Task RegisterUser_Should_Throw_When_Email_Already_Exists()
        {
            // Arrange
            var request = _fixture.Create<RegisterRequestDTO>();

            var existingUser = _fixture.Create<ApplicationUser>();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(existingUser);

            // Act
            Func<Task> act = async () => await _service.RegisterUser(request);

            // Assert

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("El correo electrónico ya esta registrado.");

            _userManagerMock.Verify(
                x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterUser_Should_Throw_When_Charge_Not_Found()
        {
            // Arrange
            var request = _fixture.Create<RegisterRequestDTO>();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _chargesGetterRepositoryMock
                .Setup(x => x.GetCharge(request.ChargeId))!
                .ReturnsAsync((ChargeResponseDTO?)null);

            // Act
            Func<Task> act = async () => await _service.RegisterUser(request);

            // Assert

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Cargo seleccionado no encontrado.");
        }

        [Fact]
        public async Task RegisterUser_Should_Throw_When_Role_Not_Found()
        {
            // Arrange
            var request = _fixture.Create<RegisterRequestDTO>();

            var charge = _fixture.Build<Charge>()
                .With(x => x.Id, request.ChargeId)
                .Create();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _chargesGetterRepositoryMock
                .Setup(x => x.GetCharge(request.ChargeId))
                .ReturnsAsync(charge.ToChargeResponseDTO());

            _roleManagerMock
                .Setup(x => x.FindByIdAsync(request.RoleId.ToString()))
                .ReturnsAsync((ApplicationRole?)null);

            // Act
            Func<Task> act = async () => await _service.RegisterUser(request);

            // Assert

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Rol seleccionado no encontrado.");
        }

        [Fact]
        public async Task RegisterUser_Should_Throw_When_Create_User_Fails()
        {
            // Arrange
            var request = _fixture.Create<RegisterRequestDTO>();

            var charge = _fixture.Build<Charge>()
                .With(x => x.Id, request.ChargeId)
                .Create();

            var role = _fixture.Build<ApplicationRole>()
                .With(x => x.Id, request.RoleId)
                .With(x => x.Name, "Admin")
                .Create();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _chargesGetterRepositoryMock
                .Setup(x => x.GetCharge(request.ChargeId))
                .ReturnsAsync(charge.ToChargeResponseDTO());

            _roleManagerMock
                .Setup(x => x.FindByIdAsync(request.RoleId.ToString()))
                .ReturnsAsync(role);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Password invalid"
                    }));

            // Act
            Func<Task> act = async () => await _service.RegisterUser(request);

            // Assert

            await act.Should()
                .ThrowAsync<RegisterUserException>();

            _userManagerMock.Verify(
                x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterUser_Should_Throw_When_AddToRole_Fails()
        {
            // Arrange
            var request = _fixture.Create<RegisterRequestDTO>();

            var charge = _fixture.Build<Charge>()
                .With(x => x.Id, request.ChargeId)
                .Create();

            var role = _fixture.Build<ApplicationRole>()
                .With(x => x.Id, request.RoleId)
                .With(x => x.Name, "Admin")
                .Create();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _chargesGetterRepositoryMock
                .Setup(x => x.GetCharge(request.ChargeId))
                .ReturnsAsync(charge.ToChargeResponseDTO());

            _roleManagerMock
                .Setup(x => x.FindByIdAsync(request.RoleId.ToString()))
                .ReturnsAsync(role);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    role.Name!))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Role error"
                    }));

            // Act
            Func<Task> act = async () => await _service.RegisterUser(request);

            // Assert

            await act.Should()
                .ThrowAsync<RegisterUserException>();

            _verificationCodeServiceMock.Verify(
                x => x.GenerateCode(It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterUser_Should_Not_Generate_Code_When_Create_Fails()
        {
            // Arrange
            var request = _fixture.Create<RegisterRequestDTO>();

            var charge = _fixture.Build<Charge>()
                .With(x => x.Id, request.ChargeId)
                .Create();

            var role = _fixture.Build<ApplicationRole>()
                .With(x => x.Id, request.RoleId)
                .With(x => x.Name, "Admin")
                .Create();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _chargesGetterRepositoryMock
                .Setup(x => x.GetCharge(request.ChargeId))
                .ReturnsAsync(charge.ToChargeResponseDTO());

            _roleManagerMock
                .Setup(x => x.FindByIdAsync(request.RoleId.ToString()))
                .ReturnsAsync(role);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            Func<Task> act = async () => await _service.RegisterUser(request);

            // Assert

            await act.Should()
                .ThrowAsync<RegisterUserException>();

            _verificationCodeServiceMock.Verify(
                x => x.GenerateCode(It.IsAny<string>()),
                Times.Never);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(It.IsAny<string>()),
                Times.Never);
        }
    }
}
