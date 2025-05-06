using ComicWebApp.BLL.Services.Intefaces;
using ComicWebApp.DAL.Models.User;
using ComicWebApp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ComicWebApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterUserDto request)
    {
        var user = await authService.RegisterAsync(request);
        if (user is null)
            return BadRequest("Username already exists");

        return Ok(user);
    }
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(LoginUserDto request)
    {
        TokenResponseDto? result = await authService.LoginAsync(request);
        if (result is null)
        {
            return BadRequest("Invalid username or password.");
        }

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequest request)
    {
        TokenResponseDto? result = await authService.RefreshTokenAsync(request);
        if (result is null || result.AccessToken is null || result.RefreshToken is null)
        {
            return BadRequest("Invalid refresh token.");
        }

        return Ok(result);
    }
}
