using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Users;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.RepositoryContracts.Users;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Tests.Users;

public class UsersGetterServiceTest
{
    private readonly UsersGetterService _service;
    private readonly Mock<IMemoryCachesService> _memoryCachesServiceMock;
    private readonly Mock<IUsersGetterRepository> _usersGetterRepositoryMock;

    public UsersGetterServiceTest()
    {
        var loggerMock = new Mock<ILogger<UsersGetterService>>();
        _memoryCachesServiceMock =
            new Mock<IMemoryCachesService>();
        _usersGetterRepositoryMock =
            new Mock<IUsersGetterRepository>();

        _service = new UsersGetterService(
            _usersGetterRepositoryMock.Object,
            loggerMock.Object,
            _memoryCachesServiceMock.Object);
    }

    [Fact]
    public async Task GetUsers_Should_Return_Users_Page()
    {
        // Arrange
        var request = new SearchSortRequestDto();

        var expected = new PageResponseDto<UserResponseDto>
        {
            Data = [
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test User",
                    Charge = "Test Charge",
                }
            ]
        };

        _memoryCachesServiceMock
            .Setup(x => x.GetOrCreateCacheList(
                Constants.CACHE_USERS,
                request,
                It.IsAny<Func<Task<PageResponseDto<UserResponseDto>>>>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetUsers(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetUsers_Should_Use_Repository_Inside_Cache_Delegate()
    {
        // Arrange
        var request = new SearchSortRequestDto();

        var expected = new PageResponseDto<UserResponseDto>();

        _usersGetterRepositoryMock
            .Setup(x => x.GetUsers(request))
            .ReturnsAsync(expected);

        _memoryCachesServiceMock
            .Setup(x => x.GetOrCreateCacheList(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<Func<Task<PageResponseDto<UserResponseDto>>>>(),
                It.IsAny<TimeSpan>()))
            .Returns<string, object, Func<Task<PageResponseDto<UserResponseDto>>>, TimeSpan>(
                async (_, _, create, _) => await create());

        // Act
        var result = await _service.GetUsers(request);

        // Assert
        result.Should().Be(expected);

        _usersGetterRepositoryMock.Verify(
            x => x.GetUsers(request),
            Times.Once);
    }

    [Fact]
    public async Task GetUser_Should_Return_User_When_Exists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expected = new UserDetailResponseDto
        {
            Id = id,
            Email = "test@test.com"
        };

        _memoryCachesServiceMock
            .Setup(x => x.GetOrCreateCache(
                $"{Constants.CACHE_USERS}_{id}",
                It.IsAny<Func<Task<UserDetailResponseDto?>>>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetUser(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetUser_Should_Throw_When_User_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();

        _memoryCachesServiceMock
            .Setup(x => x.GetOrCreateCache(
                $"{Constants.CACHE_USERS}_{id}",
                It.IsAny<Func<Task<UserDetailResponseDto?>>>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync((UserDetailResponseDto?)null);

        // Act
        Func<Task> act = async () =>
            await _service.GetUser(id);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Usuario inexistente.");
    }

    [Fact]
    public async Task GetUser_Should_Call_Cache_Service_Correctly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expected = new UserDetailResponseDto
        {
            Id = id
        };

        _memoryCachesServiceMock
            .Setup(x => x.GetOrCreateCache(
                It.IsAny<string>(),
                It.IsAny<Func<Task<UserDetailResponseDto?>>>(),
                It.IsAny<TimeSpan>()))
            .ReturnsAsync(expected);

        // Act
        await _service.GetUser(id);

        // Assert
        _memoryCachesServiceMock.Verify(
            x => x.GetOrCreateCache(
                $"{Constants.CACHE_USERS}_{id}",
                It.IsAny<Func<Task<UserDetailResponseDto?>>>(),
                It.Is<TimeSpan>(t =>
                    t == TimeSpan.FromMinutes(5))),
            Times.Once);
    }

    [Fact]
    public async Task GetUser_Should_Use_Repository_Inside_Cache_Delegate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expected = new UserDetailResponseDto
        {
            Id = id
        };

        _usersGetterRepositoryMock
            .Setup(x => x.GetUser(id))
            .ReturnsAsync(expected);

        _memoryCachesServiceMock
            .Setup(x => x.GetOrCreateCache(
                It.IsAny<string>(),
                It.IsAny<Func<Task<UserDetailResponseDto?>>>(),
                It.IsAny<TimeSpan>()))
            .Returns<string, Func<Task<UserDetailResponseDto?>>, TimeSpan>(
                async (_, create, _) => await create());

        // Act
        var result = await _service.GetUser(id);

        // Assert
        result.Should().BeEquivalentTo(expected);

        _usersGetterRepositoryMock.Verify(
            x => x.GetUser(id),
            Times.Once);
    }
}