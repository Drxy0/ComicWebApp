using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels.Enums;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeries;

public class GetComicSeries
{
    public class Endpnoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("comic-series/{id:guid}", Handler)
                .WithTags("Comic Series");
        }
    }

    public static async Task<IResult> Handler([FromRoute] Guid id, AppDbContext context)
    {
        ComicSeriesModel? comicSeries = await context.ComicSeries
            .Include(cs => cs.Metadata)
            .Include(cs => cs.Stats)
            .Include(cs => cs.Chapters)
                .ThenInclude(ch => ch.Pages)
            .FirstOrDefaultAsync(cs => cs.Id == id);

        if (comicSeries is null)
        {
            return Results.NotFound($"Comic Series with Id {id} not found");
        }

        ComicSeriesMetadataDto metadataDto = new ComicSeriesMetadataDto(
            comicSeries.Metadata.Id,
            comicSeries.Metadata.Title,
            comicSeries.Metadata.Author,
            comicSeries.Metadata.Artist,
            comicSeries.Metadata.YearOfRelease,
            comicSeries.Metadata.Writer,
            comicSeries.Metadata.Penciler,
            comicSeries.Metadata.Inker,
            comicSeries.Metadata.Colorist,
            comicSeries.Metadata.Description,
            comicSeries.Metadata.OriginalLanguage,
            comicSeries.Metadata.PublicationStatus,
            comicSeries.Metadata.Genres ?? new List<Genre>(),
            comicSeries.Metadata.Themes ?? new List<Theme>()
        );

        ComicSeriesAppStatsDto statsDto = new ComicSeriesAppStatsDto(
            comicSeries.Stats.Id,
            comicSeries.Stats.Rating,
            comicSeries.Stats.ReviewCount,
            comicSeries.Stats.NumberOfReaders,
            comicSeries.Stats.CompletionRate,
            comicSeries.Stats.DropRate
        );

        List<ChapterResponse> chapters = comicSeries.Chapters?
            .OrderBy(ch => ch.Number)
            .Select(ch => new ChapterResponse(
                ch.Title,
                ch.Number,
                ch.Id,
                ch.SeriesId,
                ch.Pages?
                    .OrderBy(p => p.PageNumber)
                    .Select(p => new ChapterFilesResponse(p.Id, p.PageNumber))
                    .ToList() ?? new List<ChapterFilesResponse>()
            ))
            .ToList() ?? new List<ChapterResponse>();

        ComicSeriesResponse response = new ComicSeriesResponse(
            comicSeries.Id,
            metadataDto,
            statsDto,
            chapters,
            comicSeries.IsVerified
        );

        return Results.Ok(response);
    }
}
