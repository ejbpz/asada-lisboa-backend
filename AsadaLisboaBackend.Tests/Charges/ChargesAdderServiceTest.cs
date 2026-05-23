using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Services.Charges;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AsadaLisboaBackend.Tests.Charges
{
    public class ChargesAdderServiceTest
    {

        private readonly Fixture _fixture;
        private readonly Mock<IChargesGetterService> _chargesGetterServiceMock;
        private readonly Mock<IChargesAdderRepository> _chargesAdderRepositoryMock;
        private readonly Mock<ILogger<ChargesAdderService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly ChargesAdderService _service;

        public ChargesAdderServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors
            .Add(new OmitOnRecursionBehavior());

            _chargesGetterServiceMock = new Mock<IChargesGetterService>();

            _chargesAdderRepositoryMock = new Mock<IChargesAdderRepository>();

            _loggerMock = new Mock<ILogger<ChargesAdderService>>();

            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();

            _service = new ChargesAdderService(
                _chargesGetterServiceMock.Object,
                _chargesAdderRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateCharge_Should_Create_Charge_Successfully()
        {
            // Arrange
            var chargeName = "Administrador";

            var response = _fixture.Build<ChargeResponseDTO>()
                .With(x => x.Name, chargeName)
                .Create();

            _chargesGetterServiceMock
                .Setup(x => x.ExistsCharge(chargeName))
                .ReturnsAsync(false);

            _chargesAdderRepositoryMock
                .Setup(x => x.CreateCharge(It.IsAny<Charge>()))
                .ReturnsAsync(response);

            // Act
            var result =
                await _service.CreateCharge(chargeName);

            // Assert
            result.Should().NotBeNull();

            result.Name.Should().Be(chargeName);

            _chargesAdderRepositoryMock.Verify(
                x => x.CreateCharge(
                    It.Is<Charge>(c =>
                        c.Name == chargeName)),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(
                    Constants.CACHE_CHARGES),
                Times.Once);
        }

        [Fact]
        public async Task CreateCharge_Should_Throw_When_Charge_Already_Exists()
        {
            // Arrange
            var chargeName = "Administrador";

            _chargesGetterServiceMock
                .Setup(x => x.ExistsCharge(chargeName))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () =>
                await _service.CreateCharge(chargeName);

            // Assert
            await act.Should()
                .ThrowAsync<CreateObjectException>()
                .WithMessage("Error al crear el cargo.");

            _chargesAdderRepositoryMock.Verify(
                x => x.CreateCharge(It.IsAny<Charge>()),
                Times.Never);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(
                    It.IsAny<string>()),
                Times.Never);
        }

    }
}
