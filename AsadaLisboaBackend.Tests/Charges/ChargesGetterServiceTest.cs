using Microsoft.Extensions.Logging;
using Moq;
using AutoFixture;
using FluentAssertions;

using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Services.Charges;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Tests.Charges
{
    public class ChargesGetterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IChargesGetterRepository> _chargesGetterRepositoryMock;
        private readonly Mock<ILogger<ChargesUpdaterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly ChargesGetterService _service;

        public ChargesGetterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors
                .Add(new OmitOnRecursionBehavior());

            _chargesGetterRepositoryMock =  new Mock<IChargesGetterRepository>();

            _loggerMock = new Mock<ILogger<ChargesUpdaterService>>();

            _memoryCachesServiceMock =  new Mock<IMemoryCachesService>();

            _service = new ChargesGetterService(
                _chargesGetterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }
        [Fact]
        public async Task GetCharges_Should_Return_Charges_Successfully()
        {
            // Arrange
            var charges = _fixture
                .CreateMany<ChargeResponseDTO>(5)
                .ToList();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache<List<ChargeResponseDTO>>(
                    Constants.CACHE_CHARGES,
                    It.IsAny<Func<Task<List<ChargeResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(charges);

            // Act
            var result = await _service.GetCharges();

            // Assert
            result.Should().NotBeNull();

            result.Should().HaveCount(5);

            result.Should().BeEquivalentTo(charges);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCache<List<ChargeResponseDTO>>(
                    Constants.CACHE_CHARGES,
                    It.IsAny<Func<Task<List<ChargeResponseDTO>>>>(),
                    It.Is<TimeSpan>(t =>
                        t == TimeSpan.FromMinutes(5))),
                Times.Once);
        }

        [Fact]
        public async Task GetCharges_Should_Log_Error_When_Exception_Occurs()
        {
            // Arrange
            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCache<List<ChargeResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<List<ChargeResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ThrowsAsync(new Exception("Cache error"));

            // Act
            Func<Task> act = async () =>
                await _service.GetCharges();

            // Assert
            await act.Should()
                .ThrowAsync<CreateObjectException>()
                .WithMessage("Error al obtener la lista de cargos.");

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
