using Microsoft.Extensions.Logging;
using NSubstitute;
using TourPlanner.Bll.Services;

namespace TourPlanner.Tests;

public class PasswordHashingServiceTests
{
    private PasswordHashingService _passwordHashingService;
    private ILogger<PasswordHashingService> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<PasswordHashingService>>();
        _passwordHashingService = new PasswordHashingService(_logger);
    }

    #region Hash Tests
    [TestCase("password123")]
    [TestCase("anotherPassword!")]
    [TestCase("P@ssw0rd")]
    [TestCase("123456")]
    [TestCase("")]
    public void Hash_ReturnsNonEmptyString(string password)
    {
        var result = _passwordHashingService.Hash(password);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [TestCase("password123")]
    [TestCase("anotherPassword!")]
    [TestCase("P@ssw0rd")]
    [TestCase("123456")]
    [TestCase("")]
    public void Hash_SamePasswordTwice_ProducesDifferentHashes(string password)
    {
        var hash1 = _passwordHashingService.Hash(password);
        var hash2 = _passwordHashingService.Hash(password);
        Assert.That(hash1, Is.Not.EqualTo(hash2));
    }

    [TestCase("password123")]
    [TestCase("anotherPassword!")]
    [TestCase("P@ssw0rd")]
    [TestCase("123456")]
    [TestCase("")]
    public void Hash_ContainsHashAndSaltSeparatedByHyphen(string password)
    {
        var hashedPassword = _passwordHashingService.Hash(password);
        var parts = hashedPassword.Split('-');
        Assert.That(parts, Has.Length.EqualTo(2));
    }
    #endregion

    #region Verify Tests
    [TestCase("password123")]
    [TestCase("anotherPassword!")]
    [TestCase("P@ssw0rd")]
    [TestCase("123456")]
    [TestCase("")]
    public void Verify_CorrectPassword_ReturnsTrue(string password)
    {
        var hash = _passwordHashingService.Hash(password);
        var result = _passwordHashingService.Verify(hash, password);
        Assert.That(result, Is.True);
    }

    [TestCase("password123", "wrongPassword")]
    [TestCase("anotherPassword!", "incorrectPassword")]
    [TestCase("P@ssw0rd", "notThePassword")]
    [TestCase("123456", "654321")]
    [TestCase("", "anyPassword")]
    public void Verify_IncorrectPassword_ReturnsFalse(string password, string wrongPassword)
    {
        var hash = _passwordHashingService.Hash(password);
        var result = _passwordHashingService.Verify(hash, wrongPassword);
        Assert.That(result, Is.False);
    }

    [TestCase("password123")]
    [TestCase("anotherPassword!")]
    [TestCase("P@ssw0rd")]
    [TestCase("123456")]
    [TestCase("")]
    public void Verify_SamePasswordTwice_BothReturnTrue(string password)
    {
        var hash1 = _passwordHashingService.Hash(password);
        var hash2 = _passwordHashingService.Hash(password);

        Assert.That(_passwordHashingService.Verify(hash1, password), Is.True);
        Assert.That(_passwordHashingService.Verify(hash2, password), Is.True);
    }

    [TestCase("notahash")]
    [TestCase("")]
    [TestCase("too-many-hyphens-in-this-hash")]
    [TestCase("ZZZZZZ-ZZZZZZ")]
    public void Verify_MalformedHash_ReturnsFalse(string malformedHash)
    {
        var result = _passwordHashingService.Verify(malformedHash, "anyPassword");
        Assert.That(result, Is.False);
    }
    #endregion
}
