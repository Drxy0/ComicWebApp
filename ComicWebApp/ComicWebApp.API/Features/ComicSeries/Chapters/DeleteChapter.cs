using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace ComicWebApp.API.Features.ComicSeries.Chapters;

public class DeleteChapter
{
    public class Endpnoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("chapter/{id:guid}", Handler)
                .WithTags("Chapters");
        }
    }

    public static async Task<IResult> Handler([FromRoute] Guid id, AppDbContext context)
    {
        ComicChapter? chapter = await context.ComicChapters.FindAsync(id);
        if (chapter is null)
        {
            return Results.NotFound($"Chapter with Id {id} not found");
        }

        // QUESTION:
        // TODO: Delete all associated files, mby it can be done in mappings or somewhere else?
        context.ComicChapters.Remove(chapter);
        await context.SaveChangesAsync();

        return Results.Ok();
    }
}
