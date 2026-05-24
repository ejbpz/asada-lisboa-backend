using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Contacts;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Contacts;

namespace AsadaLisboaBackend.Tests.Contacts
{
    public class ContactsAdderServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IContactsAdderRepository> _contactsAdderRepositoryMock;
        private readonly Mock<ILogger<ContactsAdderService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly ContactsAdderService _service;

        public ContactsAdderServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _contactsAdderRepositoryMock = new Mock<IContactsAdderRepository>();

            _loggerMock = new Mock<ILogger<ContactsAdderService>>();

            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();

            _service = new ContactsAdderService(
                _contactsAdderRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateContact_Should_Create_Contact_Successfully()
        {
            // Arrange
            var request = _fixture.Build<ContactRequestDTO>()
                .With(x => x.Order, 1)
                .With(x => x.Value, "test@gmail.com")
                .With(x => x.ContactType, "Email")
                .Create();

            var createdContact = _fixture.Build<Contact>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.Order, request.Order)
                .With(x => x.Value, request.Value)
                .With(x => x.ContactType, request.ContactType)
                .Create();

            _contactsAdderRepositoryMock
                .Setup(x => x.CreateContact(It.IsAny<Contact>()))
                .ReturnsAsync(createdContact);

            // Act
            var result = await _service.CreateContact(request);

            // Assert
            result.Should().NotBeNull();

            result.Order.Should().Be(request.Order);
            result.Value.Should().Be(request.Value);
            result.ContactType.Should().Be(request.ContactType);

            _contactsAdderRepositoryMock.Verify(
                x => x.CreateContact(It.Is<Contact>(c =>
                    c.Order == request.Order &&
                    c.Value == request.Value &&
                    c.ContactType == request.ContactType
                )),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_CONTACTS),
                Times.Once);
        }

        [Fact]
        public async Task CreateContact_Should_Throw_When_Repository_Fails()
        {
            // Arrange
            var request = _fixture.Create<ContactRequestDTO>();

            _contactsAdderRepositoryMock
                .Setup(x => x.CreateContact(It.IsAny<Contact>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () => await _service.CreateContact(request);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database error");

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(It.IsAny<string>()),
                Times.Never);

            _contactsAdderRepositoryMock.Verify(
                x => x.CreateContact(It.IsAny<Contact>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateContact_Should_Call_Logger_When_Success()
        {
            // Arrange
            var request = _fixture.Create<ContactRequestDTO>();

            var createdContact = _fixture.Build<Contact>()
                .With(x => x.Order, request.Order)
                .With(x => x.Value, request.Value)
                .With(x => x.ContactType, request.ContactType)
                .Create();

            _contactsAdderRepositoryMock
                .Setup(x => x.CreateContact(It.IsAny<Contact>()))
                .ReturnsAsync(createdContact);

            // Act
            await _service.CreateContact(request);

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
        public async Task CreateContact_Should_Call_Logger_When_Error()
        {
            // Arrange
            var request = _fixture.Create<ContactRequestDTO>();

            _contactsAdderRepositoryMock
                .Setup(x => x.CreateContact(It.IsAny<Contact>()))
                .ThrowsAsync(new Exception("Internal error"));

            // Act
            Func<Task> act = async () => await _service.CreateContact(request);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }
    }
}