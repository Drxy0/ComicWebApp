using Microsoft.AspNetCore.Mvc;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity;
using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.Users.UserModels;

namespace ComicWebApp.API.Features.Auth;

public static class Login
{
    public record Request(string Email, string Password);
    public record Response(string AccessToken, string RefreshToken);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/login", Handler)
                .WithTags(Tags.Auth);
        }
    }

    public static async Task<IResult> Handler(Request request, AppDbContext context, TokenProvider tokenProvider)
    {
        User? user = await context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
        if (user is null)
            throw new Exception("User not found");

        var verificationResult = new PasswordHasher().VerifyHashedPassword(user.PasswordHash, request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            return Results.BadRequest("Invalid username or password.");

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

        return Results.Ok(new Response(token, refreshToken.Token));
    }
}
