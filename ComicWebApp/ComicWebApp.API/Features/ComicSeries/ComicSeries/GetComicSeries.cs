using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

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
        ComicSeriesModel? comic = await context.ComicSeries.FindAsync(id);
        if (comic is null)
        {
            return Results.NotFound($"Comic Series with Id {id} not found");
        }

        return Results.Ok(comic);
    }
}
