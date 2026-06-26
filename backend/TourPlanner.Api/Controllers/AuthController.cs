using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourPlanner.Api.Dtos;
using TourPlanner.Api.Services;
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

        public AuthController(
            ITokenService tokenService,
            IPasswordHashingService passwordHashingService,
            IUserService userService
        )
        {
            _tokenService = tokenService;
            _passwordHashingService = passwordHashingService;
            _userService = userService;
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
            catch (Exception) //TODO: Catch specific exception for user already exists
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

                return Ok();
            }
            catch (Exception e) when (e is InvalidDataException) // TODO: EXPAND EXCEPTION TO INCLUDE USER NOT FOUND
            {
                return Unauthorized("Invalid credentials");
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Ok();
        }

        [HttpPost("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            // TODO: Implement this method to return the current user based on the token in the cookie
            throw new NotImplementedException();
        }
    }
}
