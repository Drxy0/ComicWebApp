using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.ComicSeries.Chapters;

public class GetChapterNumberAndTitle
{
    public record Response(float Number, string? Title);

    public class Endpnoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("chapter/{id:guid}/number-title", Handler)
                .WithTags(Tags.Chapters);
        }
    }

    public static async Task<IResult> Handler([FromRoute] Guid id, AppDbContext context)
    {
        ComicChapter? chapter = await context.ComicChapters
            .AsNoTracking()
            .Include(c => c.Pages)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (chapter is null)
        {
            return Results.NotFound($"Chapter with Id {id} not found");
        }

        return Results.Ok(new Response(chapter.Number, chapter.Title));
    }
}
