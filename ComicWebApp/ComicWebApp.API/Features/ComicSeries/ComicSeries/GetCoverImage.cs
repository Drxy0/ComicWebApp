using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeries;

public class GetCoverImage
{
    public class Endpnoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("comic-series/{id:guid}/cover-image", Handler)
                .WithTags(Tags.ComicSeries);
        }
    }
    public static async Task<IResult> Handler([FromRoute] Guid id, AppDbContext context, IWebHostEnvironment env)
    {
        ComicSeriesModel? comicSeries = await context.ComicSeries
            .AsNoTracking()
            .Include(cs => cs.Metadata)
            .FirstOrDefaultAsync(cs => cs.Id == id);

        if (comicSeries?.Metadata?.CoverImageUrl is null)
        {
            return Results.NotFound();
        }

        string filePath = Path.Combine(env.WebRootPath, comicSeries.Metadata.CoverImageUrl);

        if (!File.Exists(filePath))
        {
            return Results.NotFound();
        }

        string extension = Path.GetExtension(filePath).ToLowerInvariant();
        string contentType = extension switch
        {
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "image/jpeg"
        };

        return Results.File(
            fileStream: File.OpenRead(filePath),
            contentType: contentType,
            enableRangeProcessing: true);
    }
}
