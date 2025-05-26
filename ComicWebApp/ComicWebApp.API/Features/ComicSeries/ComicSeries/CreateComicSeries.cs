using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeries.Dtos;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels.Enums;
using ComicWebApp.API.Infrastructure.Data;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeries;

public static class CreateComicSeries
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
        PublicationStatus? PublicationStatus,
        List<Genre>? Genres,
        List<Theme>? Themes
    );

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("comic-series/create", Handler)
                .WithTags("Comic Series");
        }
    }

    public static async Task<IResult> Handler(Request request, AppDbContext context, IWebHostEnvironment env)
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
            Title = request.Title,
            Description = request.Description,
            OriginalLanguage = request.OriginalLanguage,
            PublicationStatus = request.PublicationStatus ?? PublicationStatus.Ongoing,
            Genres = request.Genres ?? new List<Genre>(),
            Themes = request.Themes ?? new List<Theme>()
        };


        ComicSeriesModel comicSeries = new ComicSeriesModel
        {
            Id = Guid.NewGuid(),
            Metadata = metadata,
            Stats = new ComicSeriesAppStats(),
            Chapters = new List<ComicChapter>(),
            IsVerified = false
        };

        if (request.CoverImage is not null)
        {
            string seriesRelativePath = Path.Combine(
               "ComicSeries",
               ComicPathHelper.GetSeriesFolderName(comicSeries)
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

        await context.ComicSeries.AddAsync(comicSeries);

        try
        {
            await context.SaveChangesAsync();

        }
        catch
        {
            return Results.InternalServerError("[ERROR] CreateComicSeries database error");
        }

        ComicSeriesMetadataDto metadataDto = new ComicSeriesMetadataDto(metadata);

        ComicSeriesResponse response = new ComicSeriesResponse(
            comicSeries.Id,
            metadataDto,
            null,
            null,
            comicSeries.IsVerified
        );

        return Results.Ok(response);
    }
}
