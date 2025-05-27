using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels.Enums;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeries;

public class UpdateComicSeriesMetadata
{
    public record Request(
        IFormFile? CoverImage,
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
                .WithTags(Tags.ComicSeries);
        }
    }

    public static async Task<IResult> Handler(AppDbContext context, Request request, Guid id, IWebHostEnvironment env)
    {
        ComicSeriesMetadata? metadata = await context.ComicSeriesMetadata
            .Include(m => m.ComicSeries)
            .FirstOrDefaultAsync(m => m.Id == id);

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

        if (request.CoverImage is not null)
        {
            string seriesRelativePath = Path.Combine(
               "ComicSeries",
               ComicPathHelper.GetSeriesFolderName(metadata.ComicSeries)
            );

            string seriesAbsolutePath = Path.Combine(env.WebRootPath, seriesRelativePath);
            Directory.CreateDirectory(seriesAbsolutePath);

            string fileName = "cover" + Path.GetExtension(request.CoverImage.FileName);

            string filePath = Path.Combine(seriesAbsolutePath, fileName);

            using (FileStream fstream = File.Create(filePath))
            {
                await request.CoverImage.CopyToAsync(fstream);
            }

            metadata.CoverImageUrl = Path.Combine(seriesRelativePath, fileName);
        }

        await context.SaveChangesAsync();

        return Results.Ok(request);
    }
}
