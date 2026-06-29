using Microsoft.Extensions.Options;
using NSubstitute;
using TourPlanner.Bll.Services;
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

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}
