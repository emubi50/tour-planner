using System.IO.IsolatedStorage;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TourPlanner.Bll.Exceptions;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Bll.Services;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Tests;

public class TourServiceTests
{
    private IUserRepository _userRepository;
    private ITourRepository _tourRepository;
    private TourService _tourService;

    private const string Username = "backfisch";
    private const int UserId = 42;

    [SetUp]
    public void Setup()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _tourRepository = Substitute.For<ITourRepository>();
        _tourService = new TourService(_tourRepository, _userRepository);

        _userRepository.GetUserByUsernameAsync(Username).Returns(new User { Id = UserId, Username = Username, HashedPassword = "hash-salt" });
    }

    #region UserNotFound Test Cases
    [TestCase("GetAllAsync")]
    [TestCase("GetByIdAsync")]
    [TestCase("CreateTourAsync")]
    [TestCase("UpdateTourAsync")]
    [TestCase("DeleteTourAsync")]
    public void Methods_UserDoesNotExist_ThrowUserNotFoundException(string methodName)
    {
        _userRepository.GetUserByUsernameAsync(Username).Returns((User?)null);

        var exception = Assert.ThrowsAsync<UserNotFoundException>(async () =>
        {
            switch (methodName)
            {
                case "GetAllAsync":
                    await _tourService.GetAllAsync(Username);
                    break;
                case "GetByIdAsync":
                    await _tourService.GetByIdAsync(Username, 1);
                    break;
                case "CreateTourAsync":
                    await _tourService.CreateTourAsync(
                        Username,
                        new Tour
                        {
                            Id = 0,
                            UserId = 0,
                            Name = "New Tour",
                            Description = "Description",
                            From = "Start",
                            To = "End",
                            TransportType = TransportType.CAR,
                            Distance = 10.0,
                            EstimatedTime = double.Parse(
                                TimeSpan.FromHours(1).TotalHours.ToString()
                            ),
                        }
                    );
                    break;
                case "UpdateTourAsync":
                    await _tourService.UpdateTourAsync(
                        Username,
                        new Tour
                        {
                            Id = 1,
                            UserId = 0,
                            Name = "Updated Tour",
                            Description = "Updated Description",
                            From = "Updated Start",
                            To = "Updated End",
                            TransportType = TransportType.BIKE,
                            Distance = 15.0,
                            EstimatedTime = double.Parse(
                                TimeSpan.FromHours(1.5).TotalHours.ToString()
                            ),
                        }
                    );
                    break;
                case "DeleteTourAsync":
                    await _tourService.DeleteTourAsync(Username, 1);
                    break;
            }
        });

        Assert.That(exception.Message, Is.EqualTo($"User with username '{Username}' not found."));
    }
    #endregion

    #region GetAllAsync Tests
    [Test]
    public async Task GetAllAsync_UserExists_ReturnsToursForUser()
    {
        var expectedTours = new List<Tour>
        {
            new Tour
            {
                Id = 1,
                UserId = UserId,
                Name = "Tour 1",
                Description = "Description 1",
                From = "Start 1",
                To = "End 1",
                TransportType = TransportType.CAR,
                Distance = 10.0,
                EstimatedTime = double.Parse(TimeSpan.FromHours(1).TotalHours.ToString()),
            },
            new Tour
            {
                Id = 2,
                UserId = UserId,
                Name = "Tour 2",
                Description = "Description 2",
                From = "Start 2",
                To = "End 2",
                TransportType = TransportType.BIKE,
                Distance = 15.0,
                EstimatedTime = double.Parse(TimeSpan.FromHours(1.5).TotalHours.ToString()),
            },
        };
        _tourRepository.GetAllByUserIdAsync(UserId).Returns(expectedTours);

        // Act
        var result = await _tourService.GetAllAsync(Username);

        // Assert
        Assert.That(result, Is.SameAs(expectedTours));
    }
    #endregion

    #region GetByIdAsync Tests
    [Test]
    public async Task GetByIdAsync_TourBelongsToUser_ReturnsTour()
    {
        // Arrange
        var tour = new Tour
        {
            Id = 1,
            UserId = UserId,
            Name = "Test Tour",
            Description = "Test Description",
            From = "Start",
            To = "End",
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = double.Parse(TimeSpan.FromHours(1).TotalHours.ToString()),
        };
        _tourRepository.GetByIdAsync(1).Returns(tour);

        // Act
        var result = await _tourService.GetByIdAsync(Username, 1);

        // Assert
        Assert.That(result, Is.EqualTo(tour));
    }

    [Test]
    public async Task GetByIdAsync_TourBelongsToDifferentUser_ReturnsNull()
    {
        // Arrange
        var tour = new Tour
        {
            Id = 1,
            UserId = UserId + 1, // Different user
            Name = "Test Tour",
            Description = "Test Description",
            From = "Start",
            To = "End",
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = double.Parse(TimeSpan.FromHours(1).TotalHours.ToString()),
        };
        _tourRepository.GetByIdAsync(1).Returns(tour);

        // Act
        var result = await _tourService.GetByIdAsync(Username, 1);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetByIdAsync_TourDoesNotExist_ReturnsNull()
    {
        // Arrange
        _tourRepository.GetByIdAsync(1).Returns((Tour?)null);
        // Act
        var result = await _tourService.GetByIdAsync(Username, 1);

        // Assert
        Assert.That(result, Is.Null);
    }
    #endregion

    #region CreateTourAsync Tests
    [Test]
    public async Task CreateTourAsync_SetsUserIdAndCallsAddAsync()
    {
        // Arrange
        var tour = new Tour
        {
            Id = 0,
            UserId = 0, // This should be overridden
            Name = "New Tour",
            Description = "New Description",
            From = "Start",
            To = "End",
            TransportType = TransportType.CAR,
            Distance = 20.0,
            EstimatedTime = double.Parse(TimeSpan.FromHours(2).TotalHours.ToString()),
        };

        // Act
        await _tourService.CreateTourAsync(Username, tour);

        // Assert
        Assert.That(tour.UserId, Is.EqualTo(UserId));
        await _tourRepository.Received(1).AddAsync(Arg.Is<Tour>(t => t == tour && t.UserId == UserId));
    }
    #endregion

    #region UpdateTourAsync Tests
    [TestCase(UserId, true)]
    [TestCase(UserId + 1, false)]
    public async Task UpdateTourAsync_TourExists_CallsUpdateOnlyWhenOwnedByUserr(int ownerId, bool expectUpdateCalled)
    {
        // Arrange
        var existingTour = new Tour
        {
            Id = 1,
            UserId = ownerId,
            Name = "Existing Tour",
            Description = "Existing Description",
            From = "Start",
            To = "End",
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = double.Parse(TimeSpan.FromHours(1).TotalHours.ToString()),
        };
        var incomingTour = new Tour
        {
            Id = 1,
            UserId = UserId,
            Name = "Updated Tour",
            Description = "Updated Description",
            From = "Updated Start",
            To = "Updated End",
            TransportType = TransportType.BIKE,
            Distance = 15.0,
            EstimatedTime = double.Parse(TimeSpan.FromHours(1.5).TotalHours.ToString()),
        };
        _tourRepository.GetByIdAsync(1).Returns(existingTour);

        // Act
        await _tourService.UpdateTourAsync(Username, incomingTour);

        // Assert
        if (expectUpdateCalled)
        {
            await _tourRepository.Received(1).UpdateAsync(incomingTour);
        }
        else
        {
            await _tourRepository.DidNotReceive().UpdateAsync(Arg.Any<Tour>());
        }
    }

    [Test]
    public async Task UpdateTourAsync_TourDoesNotExist_DoesNotCallUpdateAndReturns()
    {
        // Arrange
        var incomingTour = new Tour
        {
            Id = 1,
            UserId = UserId,
            Name = "Updated Tour",
            Description = "Updated Description",
            From = "Updated Start",
            To = "Updated End",
            TransportType = TransportType.BIKE,
            Distance = 15.0,
            EstimatedTime = double.Parse(TimeSpan.FromHours(1.5).TotalHours.ToString()),
        };
        _tourRepository.GetByIdAsync(1).Returns((Tour?)null);
        
        // Act
        await _tourService.UpdateTourAsync(Username, incomingTour);
        
        // Assert
        await _tourRepository.DidNotReceive().UpdateAsync(Arg.Any<Tour>());
    }

    [TestCase(UserId)]
    [TestCase(UserId + 1)]
    [TestCase(9999)]
    public async Task UpdateTourAsync_CallerTriesToChangeUserId_UserIdIsNotChanged(int newOwnerId)
    {
        var existingTour = new Tour
        {
            Id = 1,
            UserId = UserId,
            Name = "Existing Tour",
            Description = "Existing Description",
            From = "Start",
            To = "End",
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = double.Parse(TimeSpan.FromHours(1).TotalHours.ToString()),
        };
        var incomingTour = new Tour
        {
            Id = 1,
            UserId = newOwnerId, // Attempt to change UserId
            Name = "Updated Tour",
            Description = "Updated Description",
            From = "Updated Start",
            To = "Updated End",
            TransportType = TransportType.BIKE,
            Distance = 15.0,
            EstimatedTime = double.Parse(TimeSpan.FromHours(1.5).TotalHours.ToString()),
        };
        _tourRepository.GetByIdAsync(1).Returns(existingTour);

        // Act
        await _tourService.UpdateTourAsync(Username, incomingTour);

        // Assert
        await _tourRepository.Received(1).UpdateAsync(Arg.Is<Tour>(t => t.UserId == UserId));
        await _tourRepository.DidNotReceive().UpdateAsync(Arg.Is<Tour>(t => t.UserId == newOwnerId && t.UserId != UserId));
    }
    #endregion

    #region DeleteTourAsync Tests
    [TestCase(UserId, true)]
    [TestCase(UserId + 1, false)]
    public async Task DeleteTourAsync_TourExists_CallsDeleteOnlyWhenOwnedByUser(int ownerId,bool expectDeleteCalled)
    {
        // Arrange
        var tour = new Tour
        {
            Id = 1,
            UserId = ownerId,
            Name = "Test Tour",
            Description = "Test Description",
            From = "Start",
            To = "End",
            TransportType = TransportType.CAR,
            Distance = 10.0,
            EstimatedTime = double.Parse(TimeSpan.FromHours(1).TotalHours.ToString()),
        };
        _tourRepository.GetByIdAsync(1).Returns(tour);

        // Act
        await _tourService.DeleteTourAsync(Username, 1);

        // Assert
        if (expectDeleteCalled)
        {
            await _tourRepository.Received(1).DeleteAsync(1);
        }
        else
        {
            await _tourRepository.DidNotReceive().DeleteAsync(Arg.Any<int>());
        }
    }

    [Test]
    public async Task DeleteTourAsync_TourDoesNotExist_DoesNotCallDeleteAndReturns()
    {
        // Arrange
        _tourRepository.GetByIdAsync(1).Returns((Tour?)null);
        // Act
        await _tourService.DeleteTourAsync(Username, 1);
        // Assert
        await _tourRepository.DidNotReceive().DeleteAsync(Arg.Any<int>());
    }
    #endregion
}
