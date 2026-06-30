using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourPlanner.Api.Dtos;
using TourPlanner.Bll.Exceptions;
using TourPlanner.Bll.Interfaces;

namespace TourPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            ITokenService tokenService,
            IPasswordHashingService passwordHashingService,
            IUserService userService,
            ILogger<AuthController> logger
        )
        {
            _tokenService = tokenService;
            _passwordHashingService = passwordHashingService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CredentialsDto credentials)
        {
            var hashedPassword = _passwordHashingService.Hash(credentials.Password);
            try
            {
                await _userService.RegisterUserAsync(credentials.Username, hashedPassword);
                return Created();
            }
            catch (UserAlreadyExistsException)
            {
                return Conflict("Username already exists");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsDto credentials)
        {
            try
            {
                var user = await _userService.GetUserByUsernameAsync(credentials.Username);

                var isPasswordValid = _passwordHashingService.Verify(
                    user.HashedPassword,
                    credentials.Password
                );
                if (!isPasswordValid)
                {
                    _logger.LogWarning(
                        "Failed login attempt (invalid password) for username {Username} from {RemoteIp}",
                        credentials.Username,
                        HttpContext.Connection.RemoteIpAddress
                    );
                    throw new InvalidDataException("Invalid credentials");
                }

                var token = new TokenDto { Token = _tokenService.GenerateToken(user) };

                Response.Cookies.Append(
                    "token",
                    token.Token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                    }
                );
                _logger.LogDebug(
                    "Auth cookie set for user {Username}, expires {Expiry}",
                    user.Username,
                    DateTimeOffset.UtcNow.AddMinutes(60)
                );
                _logger.LogInformation("User {Username} logged in successfully", user.Username);

                return Ok();
            }
            catch (Exception e) when (e is InvalidDataException || e is UserNotFoundException)
            {
                return Unauthorized("Invalid credentials");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity?.Name;
            Response.Cookies.Delete("token");
            _logger.LogInformation("User {Username} logged out", username ?? "unknown");
            return Ok();
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var username = User.Identity?.Name;
            if (username == null)
            {
                _logger.LogDebug("Me endpoint called with no authenticated identity");
                return Unauthorized();
            }
            try
            {
                var user = await _userService.GetUserByUsernameAsync(username);
                return Ok(new { user.Username });
            }
            catch (UserNotFoundException)
            {
                _logger.LogWarning(
                    "Authenticated request for username {Username} but no matching user record exists",
                    username
                );
                return Unauthorized();
            }
        }
    }
}
