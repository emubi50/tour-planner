using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TourPlanner.Bll.Exceptions;
using TourPlanner.Bll.Services;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;
using TourPlanner.Models.Enums;

namespace TourPlanner.Tests;

public class TourLogServiceTests
{
    private ITourLogRepository _tourLogRepository;
    private ITourRepository _tourRepository;
    private IUserRepository _userRepository;
    private ILogger<TourLogService> _logger;
    private TourLogService _tourLogService;

    private readonly User User = new()
    {
        Id = 42,
        Username = "testuser",
        HashedPassword = "hashedpassword",
    };

    private const int UserId = 42;
    private const int TourId = 1;
    private const int TourLogId = 100;

    [SetUp]
    public void Setup()
    {
        _tourLogRepository = Substitute.For<ITourLogRepository>();
        _tourRepository = Substitute.For<ITourRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _logger = Substitute.For<ILogger<TourLogService>>();

        _tourLogService = new TourLogService(
            _tourLogRepository,
            _tourRepository,
            _userRepository,
            _logger
        );
    }

    #region GetAllAsync Tests
    [Test]
    public void GetAllAsync_UnknownUser_ThrowsUserNotFoundException()
    {
        // Arrange & Act
        _userRepository.GetUserByUsernameAsync(User.Username).Returns((User?)null);

        // Assert
        Assert.ThrowsAsync<UserNotFoundException>(async () =>
        {
            await _tourLogService.GetAllAsync(User.Username, TourId);
        });
    }

    [Test]
    public async Task GetAllAsync_TourMissingOrNotOwned_ReturnsEmptyList()
    {
        // Arrange
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);
        _tourRepository.GetByIdAsync(TourId).Returns((Tour?)null);

        // Act
        var result = await _tourLogService.GetAllAsync(User.Username, TourId);

        // Assert
        Assert.That(result, Is.Empty);
        await _tourLogRepository.DidNotReceive().GetAllByTourIdAsync(Arg.Any<int>());
    }

    [Test]
    public async Task GetAllAsync_TourOwnedByUser_ReturnsLogsFromRepository()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        // Arrange
        var tour = new Tour
        {
            Id = TourId,
            UserId = UserId,
            Name = "Test Tour",
            Description = "Test Description",
            From = location,
            To = location,
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = TimeSpan.FromHours(1).TotalHours,
            RouteInformation = String.Empty,
        };
        var tourLogs = new List<TourLog>
        {
            new TourLog
            {
                Id = TourLogId,
                TourId = TourId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Comment = "Great tour!",
                Difficulty = 3,
                TotalDistance = 10,
                TotalTime = TimeSpan.FromHours(1).TotalHours,
                Rating = 5,
            },
            new TourLog
            {
                Id = TourLogId + 1,
                TourId = TourId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Comment = "Not bad.",
                Difficulty = 2,
                TotalDistance = 5,
                TotalTime = TimeSpan.FromHours(0.5).TotalHours,
                Rating = 4,
            },
        };
        _tourRepository.GetByIdAsync(TourId).Returns(tour);
        _tourLogRepository.GetAllByTourIdAsync(TourId).Returns(tourLogs);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act
        var result = await _tourLogService.GetAllAsync(User.Username, TourId);

        // Assert
        Assert.That(result, Is.EqualTo(tourLogs));
    }
    #endregion

    #region GetByIdAsync Tests
    [Test]
    public async Task GetByIdAsync_TourMissingOrNotOwned_ReturnsNull()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        // Arrange
        var tour = new Tour
        {
            Id = TourId,
            UserId = UserId + 1, // Different user
            Name = "Test Tour",
            Description = "Test Description",
            From = location,
            To = location,
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = TimeSpan.FromHours(1).TotalHours,
            RouteInformation = String.Empty,
        };

        _tourRepository.GetByIdAsync(TourId).Returns(tour);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act
        var result = await _tourLogService.GetByIdAsync(User.Username, TourId, TourLogId);

        // Assert
        Assert.That(result, Is.Null);
        await _tourLogRepository.DidNotReceive().GetByIdAsync(Arg.Any<int>());
    }

    [Test]
    public async Task GetByIdAsync_TourLogMissingOrWrongTour_ReturnsNull()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        // Arrange
        var tour = new Tour
        {
            Id = TourId,
            UserId = UserId,
            Name = "Test Tour",
            Description = "Test Description",
            From = location,
            To = location,
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = TimeSpan.FromHours(1).TotalHours,
            RouteInformation = String.Empty,
        };
        var tourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId + 1, // Different tour
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Great tour!",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = TimeSpan.FromHours(1).TotalHours,
            Rating = 5,
        };

        _tourRepository.GetByIdAsync(TourId).Returns(tour);
        _tourLogRepository.GetByIdAsync(TourLogId).Returns(tourLog);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act
        var result = await _tourLogService.GetByIdAsync(User.Username, TourId, TourLogId);

        // Assert
        Assert.That(result, Is.Null);
        await _tourLogRepository.Received(1).GetByIdAsync(TourLogId);
    }

    [Test]
    public async Task GetByIdAsync_ValidTourAndTourLog_ReturnsTourLog()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        // Arrange
        var tour = new Tour
        {
            Id = TourId,
            UserId = UserId,
            Name = "Test Tour",
            Description = "Test Description",
            From = location,
            To = location,
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = TimeSpan.FromHours(1).TotalHours,
            RouteInformation = String.Empty,
        };
        var tourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Great tour!",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = TimeSpan.FromHours(1).TotalHours,
            Rating = 5,
        };

        _tourRepository.GetByIdAsync(TourId).Returns(tour);
        _tourLogRepository.GetByIdAsync(TourLogId).Returns(tourLog);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act
        var result = await _tourLogService.GetByIdAsync(User.Username, TourId, TourLogId);

        // Assert
        Assert.That(result, Is.EqualTo(tourLog));
    }
    #endregion

    #region CreateTourLogAsync Tests
    [Test]
    public async Task CreateTourLogAsync_TourMissingOrNotOwned_ThrowsTourNotFoundException()
    {
        // Arrange
        var tourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Great tour!",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = TimeSpan.FromHours(1).TotalHours,
            Rating = 5,
        };

        _tourRepository.GetByIdAsync(TourId).Returns((Tour?)null);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act & Assert
        Assert.ThrowsAsync<TourNotFoundException>(async () =>
        {
            await _tourLogService.CreateTourLogAsync(User.Username, tourLog);
        });
    }

    [Test]
    public async Task CreateTourLogAsync_ValidTour_AddsLogViaRepository()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        // Arrange
        var tour = new Tour
        {
            Id = TourId,
            UserId = UserId,
            Name = "Test Tour",
            Description = "Test Description",
            From = location,
            To = location,
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = TimeSpan.FromHours(1).TotalHours,
            RouteInformation = String.Empty,
        };
        var tourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Great tour!",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = TimeSpan.FromHours(1).TotalHours,
            Rating = 5,
        };

        _tourRepository.GetByIdAsync(TourId).Returns(tour);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act
        await _tourLogService.CreateTourLogAsync(User.Username, tourLog);

        // Assert
        await _tourLogRepository.Received(1).AddAsync(tourLog);
    }
    #endregion

    #region UpdateTourLogAsync Tests
    [Test]
    public async Task UpdateTourLogAsync_TourMissingOrNotOwned_ReturnsFalse()
    {
        // Arrange
        var tourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Great tour!",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = TimeSpan.FromHours(1).TotalHours,
            Rating = 5,
        };

        _tourRepository.GetByIdAsync(TourId).Returns((Tour?)null);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act
        var result = await _tourLogService.UpdateTourLogAsync(User.Username, tourLog);

        // Assert
        Assert.That(result, Is.False);
        await _tourLogRepository.DidNotReceive().UpdateAsync(Arg.Any<TourLog>());
    }

    [Test]
    public async Task UpdateTourLogAsync_ExistingLogMissingOrWrongTour_ReturnsFalse()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        // Arrange
        var tour = new Tour
        {
            Id = TourId,
            UserId = UserId,
            Name = "Test Tour",
            Description = "Test Description",
            From = location,
            To = location,
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = TimeSpan.FromHours(1).TotalHours,
            RouteInformation = String.Empty,
        };
        var oldTourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Great tour!",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = TimeSpan.FromHours(1).TotalHours,
            Rating = 5,
        };
        var newTourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId + 1, // Different tour
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Great tour!",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = TimeSpan.FromHours(1).TotalHours,
            Rating = 5,
        };

        _tourRepository.GetByIdAsync(TourId).Returns(tour);
        _tourRepository.GetByIdAsync(TourId + 1).Returns((Tour?)null);
        _tourLogRepository.GetByIdAsync(TourLogId).Returns(oldTourLog);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act
        var result = await _tourLogService.UpdateTourLogAsync(User.Username, newTourLog);

        // Assert
        Assert.That(result, Is.False);
        await _tourLogRepository.DidNotReceive().UpdateAsync(Arg.Any<TourLog>());
    }

    [Test]
    public async Task UpdateTourLogAsync_RepositoryThrowsKeyNotFound_ReturnsFalse()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        // Arrange
        var tour = new Tour
        {
            Id = TourId,
            UserId = UserId,
            Name = "Test Tour",
            Description = "Test Description",
            From = location,
            To = location,
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = TimeSpan.FromHours(1).TotalHours,
            RouteInformation = String.Empty,
        };
        var oldTourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Great tour!",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = TimeSpan.FromHours(1).TotalHours,
            Rating = 5,
        };
        var newTourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Updated comment",
            Difficulty = 4,
            TotalDistance = 12,
            TotalTime = TimeSpan.FromHours(1.5).TotalHours,
            Rating = 4,
        };

        _tourRepository.GetByIdAsync(TourId).Returns(tour);
        _tourLogRepository.GetByIdAsync(TourLogId).Returns(oldTourLog);
        _tourLogRepository.UpdateAsync(newTourLog).ThrowsAsync(new KeyNotFoundException());
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act
        var result = await _tourLogService.UpdateTourLogAsync(User.Username, newTourLog);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateTourLogAsync_ValidUpdate_ReturnsTrue()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        // Arrange
        var tour = new Tour
        {
            Id = TourId,
            UserId = UserId,
            Name = "Test Tour",
            Description = "Test Description",
            From = location,
            To = location,
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = TimeSpan.FromHours(1).TotalHours,
            RouteInformation = String.Empty,
        };
        var oldTourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Great tour!",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = TimeSpan.FromHours(1).TotalHours,
            Rating = 5,
        };
        var newTourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Updated comment",
            Difficulty = 4,
            TotalDistance = 12,
            TotalTime = TimeSpan.FromHours(1.5).TotalHours,
            Rating = 4,
        };

        _tourRepository.GetByIdAsync(TourId).Returns(tour);
        _tourLogRepository.GetByIdAsync(TourLogId).Returns(oldTourLog);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);
        // Act
        var result = await _tourLogService.UpdateTourLogAsync(User.Username, newTourLog);

        // Assert
        Assert.That(result, Is.True);
        await _tourLogRepository.Received(1).UpdateAsync(newTourLog);
    }
    #endregion

    #region DeleteTourLogAsync Tests
    [Test]
    public async Task DeleteTourLogAsync_TourMissingOrNotOwned_ReturnsFalse()
    {
        // Arrange
        _tourRepository.GetByIdAsync(TourId).Returns((Tour?)null);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act
        var result = await _tourLogService.DeleteTourLogAsync(User.Username, TourId, TourLogId);

        // Assert
        Assert.That(result, Is.False);
        await _tourLogRepository.DidNotReceive().DeleteAsync(Arg.Any<int>());
    }

    [Test]
    public async Task DeleteTourLogAsync_LogMissingOrWrongTour_ReturnsFalse()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        // Arrange
        var tour = new Tour
        {
            Id = TourId,
            UserId = UserId,
            Name = "Test Tour",
            Description = "Test Description",
            From = location,
            To = location,
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = TimeSpan.FromHours(1).TotalHours,
            RouteInformation = String.Empty,
        };
        var tourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId + 1, // Different tour
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Great tour!",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = TimeSpan.FromHours(1).TotalHours,
            Rating = 5,
        };

        _tourRepository.GetByIdAsync(TourId).Returns(tour);
        _tourRepository.GetByIdAsync(TourId + 1).Returns((Tour?)null);
        _tourLogRepository.GetByIdAsync(TourLogId).Returns(tourLog);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act
        var result = await _tourLogService.DeleteTourLogAsync(User.Username, TourId, TourLogId);

        // Assert
        Assert.That(result, Is.False);
        await _tourLogRepository.DidNotReceive().DeleteAsync(Arg.Any<int>());
    }

    [Test]
    public async Task DeleteTourLogAsync_RepositoryThrowsKeyNotFound_ReturnsFalse()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        // Arrange
        var tour = new Tour
        {
            Id = TourId,
            UserId = UserId,
            Name = "Test Tour",
            Description = "Test Description",
            From = location,
            To = location,
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = TimeSpan.FromHours(1).TotalHours,
            RouteInformation = String.Empty,
        };

        _tourRepository.GetByIdAsync(TourId).Returns(tour);
        _tourLogRepository.DeleteAsync(TourLogId).ThrowsAsync(new KeyNotFoundException());
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);

        // Act
        var result = await _tourLogService.DeleteTourLogAsync(User.Username, TourId, TourLogId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteTourLogAsync_ValidDelete_ReturnsTrue()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        // Arrange
        var tour = new Tour
        {
            Id = TourId,
            UserId = UserId,
            Name = "Test Tour",
            Description = "Test Description",
            From = location,
            To = location,
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = TimeSpan.FromHours(1).TotalHours,
            RouteInformation = String.Empty,
        };
        var tourLog = new TourLog
        {
            Id = TourLogId,
            TourId = TourId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Comment = "Great tour!",
            Difficulty = 3,
            TotalDistance = 10,
            TotalTime = TimeSpan.FromHours(1).TotalHours,
            Rating = 5,
        };

        _tourRepository.GetByIdAsync(TourId).Returns(tour);
        _tourLogRepository.GetByIdAsync(TourLogId).Returns(tourLog);
        _userRepository.GetUserByUsernameAsync(User.Username).Returns(User);
        // Act
        var result = await _tourLogService.DeleteTourLogAsync(User.Username, TourId, TourLogId);

        // Assert
        Assert.That(result, Is.True);
        await _tourLogRepository.Received(1).DeleteAsync(TourLogId);
    }
    #endregion
}
