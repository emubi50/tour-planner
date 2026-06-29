using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TourPlanner.Bll.Exceptions;
using TourPlanner.Bll.Services;
using TourPlanner.Dal.Exceptions;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Tests;

public class UserServiceTests
{
    private IUserRepository _userRepository;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _userService = new UserService(_userRepository);
    }

    #region TestCaseData Sources
    private static IEnumerable<TestCaseData> UsernamesCases
    {
        get
        {
            yield return new TestCaseData("alice");
            yield return new TestCaseData("bob");
            yield return new TestCaseData("");
        }
    }

    private static IEnumerable<TestCaseData> RegisterUserCases
    {
        get
        {
            yield return new TestCaseData("alice", "hashedPwd1");
            yield return new TestCaseData("bob", "hashedPwd2");
        }
    }
    #endregion

    #region GetUserByUsernameAsync Tests
    [TestCaseSource(nameof(UsernamesCases))]
    public async Task GetUserByUsernameAsync_UserExists_ReturnsUser(string username)
    {
        // Arrange
        var expectedUser = new User { Username = username, HashedPassword = "hash-salt" };
        _userRepository.GetUserByUsernameAsync(username).Returns(expectedUser);

        // Act
        var result = await _userService.GetUserByUsernameAsync(username);

        // Assert
        Assert.That(result, Is.SameAs(expectedUser));
    }

    [TestCaseSource(nameof(UsernamesCases))]
    public void GetUserByUsernameAsync_UserDoesNotExist_ThrowsUserNotFoundException(string username)
    {
        // Arrange
        _userRepository.GetUserByUsernameAsync(username).Returns((User?)null);

        // Act
        var ex = Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserByUsernameAsync(username));

        // Assert
        Assert.That(ex.Message, Is.EqualTo($"User with username '{username}' not found."));
    }
    #endregion

    #region RegisterUserAsync Tests
    [TestCaseSource(nameof(RegisterUserCases))]
    public async Task RegisterUserAsync_NewUser_InsertsUserWithCorrectData(string username, string hashedPassword)
    {
        // Act
        await _userService.RegisterUserAsync(username, hashedPassword);

        // Assert
        await _userRepository.Received(1).InsertUserAsync(Arg.Is<User>(u => u.Username == username && u.HashedPassword == hashedPassword));
    }

    [TestCaseSource(nameof(UsernamesCases))]
    public void RegisterUserAsync_DuplicateUsername_ThrowsUserAlreadyExistsException(string username)
    {
        // Arrange
        var duplicateKeyException = new DuplicateKeyException();
        _userRepository.InsertUserAsync(Arg.Any<User>()).ThrowsAsync(duplicateKeyException);

        // Act
        var ex = Assert.ThrowsAsync<UserAlreadyExistsException>(() => _userService.RegisterUserAsync(username, "testpass"));

        // Assert
        Assert.That(ex.Message, Does.Contain(username));
        Assert.That(ex.InnerException, Is.SameAs(duplicateKeyException));
    }
    #endregion
}
