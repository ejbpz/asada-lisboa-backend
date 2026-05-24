using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Contacts;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Contacts;

namespace AsadaLisboaBackend.Tests.Contacts
{
    public class ContactsDeleterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IContactsDeleterRepository> _contactsDeleterRepositoryMock;
        private readonly Mock<ILogger<ContactsDeleterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly ContactsDeleterService _service;

        public ContactsDeleterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _contactsDeleterRepositoryMock = new Mock<IContactsDeleterRepository>();

            _loggerMock = new Mock<ILogger<ContactsDeleterService>>();

            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();

            _service = new ContactsDeleterService(
                _contactsDeleterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task DeleteContact_Should_Delete_Contact_Successfully()
        {
            // Arrange
            var id = Guid.NewGuid();

            _contactsDeleterRepositoryMock
                .Setup(x => x.DeleteContact(id))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteContact(id);

            // Assert
            _contactsDeleterRepositoryMock.Verify(
                x => x.DeleteContact(id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(Constants.CACHE_CONTACTS, id),
                Times.Once);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(Constants.CACHE_CONTACTS),
                Times.Once);
        }

        [Fact]
        public async Task DeleteContact_Should_Call_Logger_When_Success()
        {
            // Arrange
            var id = Guid.NewGuid();

            _contactsDeleterRepositoryMock
                .Setup(x => x.DeleteContact(id))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteContact(id);

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
        public async Task DeleteContact_Should_Throw_When_Delete_Fails()
        {
            // Arrange
            var id = Guid.NewGuid();

            _contactsDeleterRepositoryMock
                .Setup(x => x.DeleteContact(id))
                .ThrowsAsync(new Exception("Delete error"));

            // Act
            Func<Task> act = async () => await _service.DeleteContact(id);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Delete error");

            _memoryCachesServiceMock.Verify(
                x => x.RemoveById(It.IsAny<string>(), It.IsAny<Guid>()),
                Times.Never);

            _memoryCachesServiceMock.Verify(
                x => x.ChangeVersion(It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteContact_Should_Call_Logger_When_Error()
        {
            // Arrange
            var id = Guid.NewGuid();

            _contactsDeleterRepositoryMock
                .Setup(x => x.DeleteContact(id))
                .ThrowsAsync(new Exception("Internal error"));

            // Act
            Func<Task> act = async () => await _service.DeleteContact(id);

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