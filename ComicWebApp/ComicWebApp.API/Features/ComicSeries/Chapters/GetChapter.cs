using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.ComicSeries.Chapters;

public class GetChapter
{
    public record GetChapterFilesResponse(Guid Id, int PageNumber);
    public record GetChapterResponse(string? Title, float Number, Guid Id, Guid SeriesId, List<GetChapterFilesResponse> Pages);
    public class Endpnoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("chapter/{id:guid}", Handler)
                .WithTags("Chapters");
        }
    }

    public static async Task<IResult> Handler([FromRoute] Guid id, AppDbContext context)
    {
        ComicChapter? chapter = await context.ComicChapters
            .Include(c => c.Pages)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (chapter is null)
        {
            return Results.NotFound($"Chapter with Id {id} not found");
        }

        GetChapterResponse response = new GetChapterResponse(
            chapter.Title,
            chapter.Number,
            chapter.Id,
            chapter.SeriesId,
            chapter.Pages
                .OrderBy(p => p.PageNumber)
                .Select(p => new GetChapterFilesResponse(
                    Id: p.Id,
                    PageNumber: p.PageNumber
                ))
                .ToList()
        );

        return Results.Ok(response);
    }
}
