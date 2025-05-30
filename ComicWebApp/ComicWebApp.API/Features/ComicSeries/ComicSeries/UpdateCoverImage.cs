using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeries;

public class UpdateCoverImage
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("comic-series/{id:guid}/cover-image", Handler)
                .WithTags(Tags.ComicSeries);
        }
    }
    public static async Task<IResult> Handler(AppDbContext context, IFormFile coverImage, Guid id, IWebHostEnvironment env)
    {
        ComicSeriesMetadata? metadata = await context.ComicSeriesMetadata
            .Include(m => m.ComicSeries)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (metadata is null)
        {
            return Results.NotFound();
        }

        string seriesRelativePath = Path.Combine(
            "ComicSeries",
            ComicPathHelper.GetSeriesFolderName(metadata.ComicSeries!)
        );

        string seriesAbsolutePath = Path.Combine(env.WebRootPath, seriesRelativePath);
        Directory.CreateDirectory(seriesAbsolutePath);

        string fileName = "cover" + Path.GetExtension(coverImage.FileName);

        string filePath = Path.Combine(seriesAbsolutePath, fileName);

        try
        {
            await using (FileStream fstream = File.Create(filePath))
            {
                await coverImage.CopyToAsync(fstream);
            }
        }
        catch (Exception ex)
        {
            return Results.InternalServerError($"Failed to save image: {ex.Message}");
        }

        metadata.CoverImageUrl = Path.Combine(seriesRelativePath, fileName);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Results.InternalServerError($"Failed to update database: {ex.Message}");
        }

        return Results.Ok();
    }
}
