using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace ComicWebApp.API.Features.ComicSeries.Pages;

public class GetPage
{

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("page/{id:guid}", Handler)
                .WithTags("Pages");
        }
    }

    public static async Task<IResult> Handler([FromRoute] Guid id, 
        AppDbContext context, IWebHostEnvironment env)
    {
        ComicPage? page = await context.ComicPages.FindAsync(id);

        if (page is null)
        {
            return Results.NotFound($"Page with Id {id} not found");
        }

        string imagePath = Path.Combine(
            env.WebRootPath,
            page.ImageUrl.TrimStart('/'));

        if (!File.Exists(imagePath))
        {
            return Results.NotFound("Page doesn't have an associated file");
        }

        string contentType = GetContentType(imagePath);
        
        return Results.File(
            imagePath,
            contentType,
            $"{page.PageNumber}{Path.GetExtension(imagePath)}");
    }

    private static string GetContentType(string path)
    {
        var extension = Path.GetExtension(path).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }
}
