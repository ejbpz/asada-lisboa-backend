using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Contacts;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Contacts;

namespace AsadaLisboaBackend.Tests.Contacts
{
    public class ContactsUpdaterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IContactsUpdaterRepository> _contactsUpdaterRepositoryMock;
        private readonly Mock<ILogger<ContactsUpdaterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly ContactsUpdaterService _service;

        public ContactsUpdaterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _contactsUpdaterRepositoryMock = new Mock<IContactsUpdaterRepository>();

            _loggerMock = new Mock<ILogger<ContactsUpdaterService>>();

            _memoryCachesServiceMock =new Mock<IMemoryCachesService>();

            _service = new ContactsUpdaterService(
                _contactsUpdaterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task UpdateContact_Should_Update_Contact_Successfully()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Build<ContactRequestDTO>()
                .With(x => x.Order, 1)
                .With(x => x.Value, "test@gmail.com")
                .With(x => x.ContactType, "Email")
                .Create();

            var updatedContact = _fixture.Build<Contact>()
                .With(x => x.Id, id)
                .With(x => x.Order, request.Order)
                .With(x => x.Value, request.Value)
                .With(x => x.ContactType, request.ContactType)
                .Create();

            _contactsUpdaterRepositoryMock
                .Setup(x => x.UpdateContact(id, request))
                .ReturnsAsync(updatedContact);

            // Act
            var result = await _service.UpdateContact(id, request);

            // Assert
            result.Should().NotBeNull();

            result.Order.Should().Be(request.Order);
            result.Value.Should().Be(request.Value);
            result.ContactType.Should().Be(request.ContactType);

            _contactsUpdaterRepositoryMock.Verify(
                x => x.UpdateContact(id, request),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(Constants.CACHE_CONTACTS, id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_CONTACTS),
                Times.Once);
        }

        [Fact]
        public async Task UpdateContact_Should_Throw_When_Contact_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Create<ContactRequestDTO>();

            _contactsUpdaterRepositoryMock
                .Setup(x => x.UpdateContact(id, request))
                .ReturnsAsync((Contact)null!);

            // Act
            Func<Task> act = async () => await _service.UpdateContact(id, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage($"No se encontró contacto para actualizar con Id: {id}");

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(It.IsAny<string>(), It.IsAny<Guid>()),
                Times.Never);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateContact_Should_Call_Logger_When_Success()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Create<ContactRequestDTO>();

            var updatedContact = _fixture.Build<Contact>()
                .With(x => x.Id, id)
                .Create();

            _contactsUpdaterRepositoryMock
                .Setup(x => x.UpdateContact(id, request))
                .ReturnsAsync(updatedContact);

            // Act
            await _service.UpdateContact(id, request);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task UpdateContact_Should_Call_Logger_When_NotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = _fixture.Create<ContactRequestDTO>();

            _contactsUpdaterRepositoryMock
                .Setup(x => x.UpdateContact(id, request))
                .ReturnsAsync((Contact)null!);

            // Act
            Func<Task> act = async () => await _service.UpdateContact(id, request);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }
    }
}