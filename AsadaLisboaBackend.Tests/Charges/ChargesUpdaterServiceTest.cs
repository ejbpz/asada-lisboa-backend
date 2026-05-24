using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Services.Charges;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Tests.Charges
{
    public class ChargesUpdaterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IChargesGetterService> _chargesGetterServiceMock;
        private readonly Mock<IChargesUpdaterRepository> _chargesUpdaterRepositoryMock;
        private readonly Mock<ILogger<ChargesUpdaterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly ChargesUpdaterService _service;

        public ChargesUpdaterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors
                .Add(new OmitOnRecursionBehavior());

            _chargesGetterServiceMock = new Mock<IChargesGetterService>();

            _chargesUpdaterRepositoryMock = new Mock<IChargesUpdaterRepository>();

            _loggerMock = new Mock<ILogger<ChargesUpdaterService>>();

            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();

            _service = new ChargesUpdaterService(
                _chargesGetterServiceMock.Object,
                _chargesUpdaterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task UpdateCharge_Should_Update_Charge_Successfully()
        {
            // Arrange
            var id = Guid.NewGuid();

            var chargeName = "Administrador";

            var response = _fixture.Build<ChargeResponseDTO>()
                .With(x => x.Name, chargeName)
                .Create();

            _chargesGetterServiceMock
                .Setup(x => x.ExistsCharge(chargeName))
                .ReturnsAsync(false);

            _chargesUpdaterRepositoryMock
                .Setup(x => x.UpdateCharge(id, chargeName))
                .ReturnsAsync(response);

            // Act
            var result = await _service.UpdateCharge(id, chargeName);

            // Assert
            result.Should().NotBeNull();

            result.Name.Should().Be(chargeName);

            _chargesUpdaterRepositoryMock.Verify(
                x => x.UpdateCharge(id, chargeName),
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
        public async Task UpdateCharge_Should_Throw_When_Charge_Already_Exists()
        {
            // Arrange
            var id = Guid.NewGuid();

            var chargeName = "Administrador";

            _chargesGetterServiceMock
                .Setup(x => x.ExistsCharge(chargeName))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _service.UpdateCharge(id, chargeName);

            // Assert
            await act.Should()
                .ThrowAsync<ExistingValueException>()
                .WithMessage("El nombre del cargo ya existe.");

            _chargesUpdaterRepositoryMock.Verify(
                x => x.UpdateCharge(
                    It.IsAny<Guid>(),
                    It.IsAny<string>()),
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
        public async Task UpdateCharge_Should_Call_Logger_Information()
        {
            // Arrange
            var id = Guid.NewGuid();

            var chargeName = "Administrador";

            var response = _fixture.Build<ChargeResponseDTO>()
                .With(x => x.Name, chargeName)
                .Create();

            _chargesGetterServiceMock
                .Setup(x => x.ExistsCharge(chargeName))
                .ReturnsAsync(false);

            _chargesUpdaterRepositoryMock
                .Setup(x => x.UpdateCharge(id, chargeName))
                .ReturnsAsync(response);

            // Act
            await _service.UpdateCharge(id, chargeName);

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
        public async Task UpdateCharge_Should_Call_Logger_Warning_When_Name_Already_Exists()
        {
            // Arrange
            var id = Guid.NewGuid();

            var chargeName = "Administrador";

            _chargesGetterServiceMock
                .Setup(x => x.ExistsCharge(chargeName))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () =>  await _service.UpdateCharge(id, chargeName);

            await act.Should()
                .ThrowAsync<ExistingValueException>();

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

        [Fact]
        public async Task UpdateCharge_Should_Update_Cache_When_Successful()
        {
            // Arrange
            var id = Guid.NewGuid();

            var chargeName = "Administrador";

            var response = _fixture.Build<ChargeResponseDTO>()
                .With(x => x.Name, chargeName)
                .Create();

            _chargesGetterServiceMock
                .Setup(x => x.ExistsCharge(chargeName))
                .ReturnsAsync(false);

            _chargesUpdaterRepositoryMock
                .Setup(x => x.UpdateCharge(id, chargeName))
                .ReturnsAsync(response);

            // Act
            await _service.UpdateCharge(id, chargeName);

            // Assert
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
    }
}