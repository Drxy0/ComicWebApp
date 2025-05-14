using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.Users.UserModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.Auth;

public static class Register
{
    public record Request(string Username, string Email, string Password);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/register", Handler)
                .WithTags("Auth");
        }
    }
    public static async Task<IResult> Handler(Request request, AppDbContext context, TokenProvider tokenProvider)
    {
        if (await context.Users.AnyAsync(u => u.Email == request.Email) ||
            await context.Users.AnyAsync(u => u.Username == request.Username))
            throw new Exception("The email or username is already in use");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = new PasswordHasher().HashPassword(request.Password)
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        if (user is null)
            return Results.BadRequest("Username already exists");

        return Results.Ok(user);
    }
}
