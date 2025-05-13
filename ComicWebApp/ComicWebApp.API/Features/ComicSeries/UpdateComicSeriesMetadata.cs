using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels.Enums;
using ComicWebApp.API.Infrastructure.Data;

namespace ComicWebApp.API.Features.ComicSeries;

public class UpdateComicSeriesMetadata
{
    public record Request(
        Guid Id,
        string? Author,
        string? Artist,
        int? YearOfRelease,
        string? Writer,
        string? Penciler,
        string? Inker,
        string? Colorist,
        string? Description,
        string? OriginalLanguage,
        PublicationStatus PublicationStatus,
        List<Genre>? Genres,
        List<Theme>? Themes
    );

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("metadata", Handler);
        }
    }

    public static async Task<IResult> Handler(AppDbContext context, Request request)
    {
        ComicSeriesMetadata? metadata = await context.ComicSeriesMetadata.FindAsync(request.Id);
        if (metadata is null)
        {
            return Results.NotFound();
        }

        metadata.Author = request.Author;
        metadata.Artist = request.Artist;
        metadata.YearOfRelease = request.YearOfRelease;
        metadata.Writer = request.Writer;
        metadata.Penciler = request.Penciler;
        metadata.Inker = request.Inker;
        metadata.Colorist = request.Colorist;
        metadata.Description = request.Description;
        metadata.OriginalLanguage = request.OriginalLanguage;
        metadata.PublicationStatus = request.PublicationStatus;
        metadata.Genres = request.Genres ?? new List<Genre>();
        metadata.Themes = request.Themes ?? new List<Theme>();

        await context.SaveChangesAsync();

        return Results.Ok(metadata);
    }
}
