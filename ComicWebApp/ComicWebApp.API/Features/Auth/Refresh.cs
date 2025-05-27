using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.Users.UserModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.Auth;

public class Refresh
{
    public record Request(Guid UserId, string RefreshToken);
    public record Response(string AccessToken, string RefreshToken);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/refresh-token", Handler)
                .WithTags(Tags.Auth);
        }
    }

    public static async Task<IResult> Handler(Request request, AppDbContext context, TokenProvider tokenProvider)
    {
        RefreshToken? refreshToken = await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == request.RefreshToken);

        if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            return Results.BadRequest("Invalid refresh token.");
        }

        string accessToken = tokenProvider.Create(refreshToken.User);

        refreshToken.Token = tokenProvider.GenerateRefreshToken();
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

        await context.SaveChangesAsync();

        return Results.Ok(new Response(accessToken, refreshToken.Token));
    }
}
