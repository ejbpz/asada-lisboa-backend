using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.Services.Charges;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Tests.Charges
{
    public class ChargesDeleterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IChargesDeleterRepository> _chargesDeleterRepositoryMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<ILogger<ChargesDeleterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly ChargesDeleterService _service;

        public ChargesDeleterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors
                .Add(new OmitOnRecursionBehavior());

            _chargesDeleterRepositoryMock =
                new Mock<IChargesDeleterRepository>();

            var store = new Mock<IUserStore<ApplicationUser>>();

            _userManagerMock =
                new Mock<UserManager<ApplicationUser>>(
                    store.Object,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!);

            _loggerMock =
                new Mock<ILogger<ChargesDeleterService>>();

            _memoryCachesServiceMock =
                new Mock<IMemoryCachesService>();

            _service = new ChargesDeleterService(
                _chargesDeleterRepositoryMock.Object,
                _userManagerMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task DeleteCharge_Should_Delete_Charge_Successfully()
        {
            // Arrange
            var id = Guid.NewGuid();

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.Id, Guid.Empty)
                .Create();

            _userManagerMock
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ReturnsAsync(user);

            _chargesDeleterRepositoryMock
                .Setup(x => x.DeleteCharge(id))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteCharge(id);

            // Assert
            _chargesDeleterRepositoryMock.Verify(
                x => x.DeleteCharge(id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(
                    Constants.CACHE_CHARGES,
                    id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(
                    Constants.CACHE_CHARGES),
                Times.Once);
        }

        [Fact]
        public async Task DeleteCharge_Should_Throw_NotFoundException_When_User_Does_Not_Exist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _userManagerMock
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            Func<Task> act = async () =>
                await _service.DeleteCharge(id);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Usuario inexistente.");

            _chargesDeleterRepositoryMock.Verify(
                x => x.DeleteCharge(It.IsAny<Guid>()),
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
        public async Task DeleteCharge_Should_Call_Logger_Information()
        {
            // Arrange
            var id = Guid.NewGuid();

            var user = _fixture.Build<ApplicationUser>()
                .With(x => x.Id, Guid.Empty)
                .Create();

            _userManagerMock
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ReturnsAsync(user);

            _chargesDeleterRepositoryMock
                .Setup(x => x.DeleteCharge(id))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteCharge(id);

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
        public async Task DeleteCharge_Should_Call_Logger_Warning_When_User_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();

            _userManagerMock
                .Setup(x => x.FindByIdAsync(id.ToString()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            Func<Task> act = async () =>
                await _service.DeleteCharge(id);

            await act.Should()
                .ThrowAsync<NotFoundException>();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
