using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Infrastructure.Data;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeries;

public class DeleteComicSeries
{
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("comic-series/{id:guid}", Handler);
        }
    }

    public static async Task<IResult> Handler(AppDbContext context, Guid id)
    {
        var existingSeries = await context.ComicSeries.FindAsync(id);

        if (existingSeries is null)
            return Results.NotFound();

        context.ComicSeries.Remove(existingSeries);

        await context.SaveChangesAsync();

        return Results.Ok();
    }
}
