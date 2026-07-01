using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using TourPlanner.Bll.Services;
using TourPlanner.Models;
using TourPlanner.Models.Options;

namespace TourPlanner.Tests;

public class TokenServiceTests
{
    private const string TestSigningKey = "ThisIsASecretSigningKeyForTestingPurposesOnly12345";
    private const string TestIssuer = "TourPlanner.Tests";
    private const string TestAudience = "TourPlanner.Tests.Audience";
    private const int TestExpirationMinutes = 60;

    private TokenService _tokenService;

    [SetUp]
    public void Setup()
    {
        var jwtSettings = new JwtSettings
        {
            SigningKey = TestSigningKey,
            Issuer = TestIssuer,
            Audience = TestAudience,
            ExpirationMinutes = TestExpirationMinutes,
        };

        var options = Substitute.For<IOptions<JwtSettings>>();
        options.Value.Returns(jwtSettings);

        _tokenService = new TokenService(options);
    }

    #region Helper Methods
    private string GenerateTestToken(string username = "testuser")
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = username,
            HashedPassword = "hashedpassword",
        };
        // Act
        return _tokenService.GenerateToken(user);
    }
    #endregion

    #region GenerateToken Tests
    [TestCase("alice")]
    [TestCase("bob")]
    [TestCase("")]
    public void GenerateToken_ReturnsWellFormedJwt(string username)
    {
        // Arrange & Act in GenerateTestToken
        Assert.DoesNotThrow(() =>
            new JwtSecurityTokenHandler().ReadToken(GenerateTestToken(username))
        );
    }

    [TestCase("alice")]
    [TestCase("bob")]
    [TestCase("")]
    public void GenerateToken_IncludesUsernameClaim(string username)
    {
        // Arrange & Act
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(GenerateTestToken(username));

        // Debug
        foreach (var claim in jwt.Claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
        }

        // Assert
        var subClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        Assert.That(subClaim, Is.Not.Null);
        Assert.That(subClaim!.Value, Is.EqualTo(username));

        var nameClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);
        Assert.That(nameClaim, Is.Not.Null);
        Assert.That(nameClaim!.Value, Is.EqualTo(username));
    }

    [Test]
    public void GenerateToken_SetsIssuerAndAudienceFromSettings()
    {
        // Arrange & Act
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(GenerateTestToken());

        // Assert
        Assert.That(jwt.Issuer, Is.EqualTo(TestIssuer));
        Assert.That(jwt.Audiences, Does.Contain(TestAudience));
    }

    [Test]
    public void GenerateToken_SetsExpirationBasedOnSettings()
    {
        // Arrange & Act
        var beforeGeneration = DateTime.UtcNow;
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(GenerateTestToken());

        // Assert
        var expectedExpiration = beforeGeneration.AddMinutes(TestExpirationMinutes);
        Assert.That(jwt.ValidTo, Is.EqualTo(expectedExpiration).Within(TimeSpan.FromSeconds(10)));
    }

    [Test]
    public void GenerateToken_IsSignedWithHmacSha256()
    {
        // Arrange & Act
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(GenerateTestToken());

        // Assert
        Assert.That(jwt.Header.Alg, Is.EqualTo(SecurityAlgorithms.HmacSha256));
    }

    [Test]
    public void GenerateToken_TokenValidatesAgainstSigningKeyFromSettings()
    {
        // Arrange
        var token = GenerateTestToken();
        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = TestIssuer,
            ValidateAudience = true,
            ValidAudience = TestAudience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestSigningKey)),
            ValidateIssuerSigningKey = true,
        };

        // Act & Assert
        Assert.DoesNotThrow(() => handler.ValidateToken(token, validationParameters, out _));
    }

    [Test]
    public void GenerateToken_TokenFailsValidationWithWrongSigningKey()
    {
        // Arrange
        var token = GenerateTestToken();
        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = TestIssuer,
            ValidateAudience = true,
            ValidAudience = TestAudience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("WrongSigningKey")),
            ValidateIssuerSigningKey = true,
        };

        // Act & Assert
        Assert.Throws<SecurityTokenSignatureKeyNotFoundException>(() =>
            handler.ValidateToken(token, validationParameters, out _)
        );
    }

    [TestCase("alice")]
    [TestCase("bob")]
    public void GenerateToken_DifferentUsers_ProduceDifferentTokens(string username)
    {
        // Arrange & Act
        var token1 = GenerateTestToken(username);
        var token2 = GenerateTestToken(username + "2");

        // Assert
        Assert.That(token1, Is.Not.EqualTo(token2));
    }
    #endregion
}
