using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels.Enums;
using ComicWebApp.API.Infrastructure.Data;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeries;

public static class CreateComicSeries
{
    public record Request(
        string? Author,
        string? Artist,
        int? YearOfRelease,
        string? Writer,
        string? Penciler,
        string? Inker,
        string? Colorist,
        string? Description,
        string? OriginalLanguage,
        PublicationStatus? PublicationStatus,
        List<Genre>? Genres,
        List<Theme>? Themes
    );

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("create", Handler);
        }
    }

    public static async Task<IResult> Handler(Request request, AppDbContext context)
    {
        ComicSeriesMetadata metadata = new ComicSeriesMetadata
        {
            Author = request.Author,
            Artist = request.Artist,
            YearOfRelease = request.YearOfRelease,
            Writer = request.Writer,
            Penciler = request.Penciler,
            Inker = request.Inker,
            Colorist = request.Colorist,
            Description = request.Description,
            OriginalLanguage = request.OriginalLanguage,
            PublicationStatus = request.PublicationStatus ?? PublicationStatus.Ongoing,
            Genres = request.Genres ?? new List<Genre>(),
            Themes = request.Themes ?? new List<Theme>()
        };

        ComicSeriesModel comicSeries = new ComicSeriesModel
        {
            Metadata = metadata,
            Stats = new ComicSeriesAppStats(),
            IsVerified = false
        };

        await context.ComicSeries.AddAsync(comicSeries);

        try
        {
            await context.SaveChangesAsync();
        }
        catch
        {
            return Results.InternalServerError("[ERROR] CreateComicSeries database error");
        }

        return Results.Ok(comicSeries);
    }

}
