using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels.Enums;
using ComicWebApp.API.Infrastructure.Data;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeries;

public class UpdateComicSeriesMetadata
{
    public record Request(
        string? Author,
        string? Artist,
        int? YearOfRelease,
        string? Writer,
        string? Penciler,
        string? Inker,
        string? Colorist,
        string Title,
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
            app.MapPut("comic-series/{id:guid}/metadata", Handler)
                .WithTags("Comic Series");
        }
    }

    public static async Task<IResult> Handler(AppDbContext context, Request request, Guid id)
    {
        ComicSeriesMetadata? metadata = await context.ComicSeriesMetadata.FindAsync(id);
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
        metadata.Title = request.Title;
        metadata.Description = request.Description;
        metadata.OriginalLanguage = request.OriginalLanguage;
        metadata.PublicationStatus = request.PublicationStatus;
        metadata.Genres = request.Genres ?? new List<Genre>();
        metadata.Themes = request.Themes ?? new List<Theme>();

        await context.SaveChangesAsync();

        return Results.Ok(request);
    }
}
