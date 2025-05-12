using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Infrastructure.Data;
using ComicWebApp.API.Models.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace ComicWebApp.API.Features.Auth;

public static class Register
{
    public record Request(string Username, string Email, string Password);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/register", Handler);
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
