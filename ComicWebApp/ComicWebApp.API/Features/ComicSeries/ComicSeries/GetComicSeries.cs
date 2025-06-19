using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;
using ComicWebApp.API.Features.ComicSeries.ComicSeries.Dtos;
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
                .WithTags(Tags.ComicSeries);
        }
    }

    public static async Task<IResult> Handler([FromRoute] Guid id, AppDbContext context)
    {
        ComicSeriesModel? comicSeries = await context.ComicSeries
            .AsNoTracking()
            .Include(cs => cs.Metadata)
            .Include(cs => cs.Stats)
            .Include(cs => cs.Chapters)
                .ThenInclude(ch => ch.Pages)
            .FirstOrDefaultAsync(cs => cs.Id == id);

        if (comicSeries is null)
        {
            return Results.NotFound($"Comic Series with Id {id} not found");
        }

        ComicSeriesMetadataDto metadataDto = new ComicSeriesMetadataDto(comicSeries.Metadata);

        ComicSeriesAppStatsDto statsDto = new ComicSeriesAppStatsDto(comicSeries.Stats);
        
        List<ChapterResponse> chapters = comicSeries.Chapters?
            .OrderBy(ch => ch.Number)
            .Select(ch => new ChapterResponse(
                ch.Title,
                ch.Number,
                ch.Language,
                ch.Id,
                ch.SeriesId,
                ch.Pages.Count()
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
