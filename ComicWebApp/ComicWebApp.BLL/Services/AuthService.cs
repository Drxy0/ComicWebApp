using ComicWebApp.BLL.Services.Intefaces;
using ComicWebApp.DAL;
using ComicWebApp.DAL.Context;
using ComicWebApp.DAL.Models.User;
using ComicWebApp.Shared.DTOs;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ComicWebApp.BLL.Services;

public class AuthService(AppDbContext context, TokenProvider tokenProvider) : IAuthService
{
    public async Task<TokenResponseDto> LoginAsync(LoginUserDto login)
    {
        User? user = await context.Users.SingleOrDefaultAsync(u => u.Email == login.Email);
        if (user is null)
            throw new Exception("User not found");

    var verificationResult = new PasswordHasher().VerifyHashedPassword(user.PasswordHash, login.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            throw new Exception("Incorrect password");

        string token = tokenProvider.Create(user);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = tokenProvider.GenerateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddHours(2)
        };

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync();

        return new TokenResponseDto(token, refreshToken.Token);

    }

    public async Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequest request)
    {
        RefreshToken? refreshToken = await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == request.RefreshToken);

        if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            throw new Exception("The refresh token has expired");
        }

        string accessToken = tokenProvider.Create(refreshToken.User);

        // QUESTION: Why do we edit the current db row and not create a new one or smth idk?
        // Like making the old one "expired" by setting a bool flag, instead of just overwriting
        refreshToken.Token = tokenProvider.GenerateRefreshToken();
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

        await context.SaveChangesAsync();

        return new TokenResponseDto(accessToken, refreshToken.Token);
    }

    // QUESTION: Why return user and not (token, refreshToken) pair like in login?
    // Nvm - userId is a parameter in /refresh-token endpoint request
    public async Task<User?> RegisterAsync(RegisterUserDto register)
    {
        if (await context.Users.AnyAsync(u => u.Email == register.Email) ||
            await context.Users.AnyAsync(u => u.Username == register.Username))
            throw new Exception("The email or username is already in use");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = register.Username,
            Email = register.Email,
            PasswordHash = new PasswordHasher().HashPassword(register.Password)
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }
}
