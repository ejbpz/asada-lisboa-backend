using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Contacts;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Contacts
{
    public class ContactsGetterServiceTest
    {
        private readonly Fixture _fixture;

        private readonly Mock<IContactsGetterRepository> _contactsGetterRepositoryMock;
        private readonly Mock<ILogger<ContactsGetterService>> _loggerMock;
        private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;

        private readonly ContactsGetterService _service;

        public ContactsGetterServiceTest()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _contactsGetterRepositoryMock = new Mock<IContactsGetterRepository>();

            _loggerMock = new Mock<ILogger<ContactsGetterService>>();

            _memoryCachesServiceMock = new Mock<IMemoryCachesService>();

            _service = new ContactsGetterService(
                _contactsGetterRepositoryMock.Object,
                _loggerMock.Object,
                _memoryCachesServiceMock.Object
            );
        }

        [Fact]
        public async Task GetContacts_Should_Return_Contacts_Successfully()
        {
            // Arrange
            var request = _fixture.Build<SearchSortRequestDTO>()
                .With(x => x.Take, 10)
                .Create();

            var contacts = _fixture
                .CreateMany<ContactResponseDTO>(5)
                .ToList();

            var pageResponse = new PageResponseDTO<ContactResponseDTO>
            {
                Data = contacts,
                Total = contacts.Count
            };

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<ContactResponseDTO>>(
                    Constants.CACHE_CONTACTS,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<ContactResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(pageResponse);

            // Act
            var result = await _service.GetContacts(request);

            // Assert
            result.Should().NotBeNull();
            result.Total.Should().Be(contacts.Count);
            result.Data.Should().HaveCount(5);

            _memoryCachesServiceMock.Verify(
                x => x.GetOrCreateCacheList<PageResponseDTO<ContactResponseDTO>>(
                    Constants.CACHE_CONTACTS,
                    request,
                    It.IsAny<Func<Task<PageResponseDTO<ContactResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Fact]
        public async Task GetContacts_Should_Call_Logger_When_Success()
        {
            // Arrange
            var request = _fixture.Build<SearchSortRequestDTO>()
                .With(x => x.Take, 10)
                .Create();

            var pageResponse = new PageResponseDTO<ContactResponseDTO>
            {
                Data = new List<ContactResponseDTO>(),
                Total = 0
            };

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<ContactResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<Func<Task<PageResponseDTO<ContactResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(pageResponse);

            // Act
            await _service.GetContacts(request);

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
        public async Task GetContacts_Should_Throw_When_Cache_Fails()
        {
            // Arrange
            var request = _fixture.Build<SearchSortRequestDTO>()
                .With(x => x.Take, 10)
                .Create();

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<ContactResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<Func<Task<PageResponseDTO<ContactResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .ThrowsAsync(new Exception("Cache error"));

            // Act
            Func<Task> act = async () => await _service.GetContacts(request);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Cache error");

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task GetContacts_Should_Call_Repository_Through_Cache_Create_Function()
        {
            // Arrange
            var request = _fixture.Create<SearchSortRequestDTO>();

            var expectedResponse = new PageResponseDTO<ContactResponseDTO>
            {
                Data = _fixture.CreateMany<ContactResponseDTO>(3).ToList(),
                Total = 3
            };

            _memoryCachesServiceMock
                .Setup(x => x.GetOrCreateCacheList<PageResponseDTO<ContactResponseDTO>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<Func<Task<PageResponseDTO<ContactResponseDTO>>>>(),
                    It.IsAny<TimeSpan>()))
                .Returns<string, object, Func<Task<PageResponseDTO<ContactResponseDTO>>>, TimeSpan>(
                    async (resource, req, create, time) =>
                    {
                        return await create();
                    });

            _contactsGetterRepositoryMock
                .Setup(x => x.GetContacts(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.GetContacts(request);

            // Assert
            result.Should().NotBeNull();
            result.Total.Should().Be(3);

            _contactsGetterRepositoryMock.Verify(
                x => x.GetContacts(request),
                Times.Once);
        }
    }
}