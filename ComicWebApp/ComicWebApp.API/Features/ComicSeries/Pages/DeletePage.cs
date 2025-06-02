using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ComicWebApp.API.Features.ComicSeries.Pages;

public class DeletePage
{
    public class Endpnoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("comic/{chapterId:guid}/{pageNumber:int}", Handler)
                .WithTags(Tags.Pages);
        }
    }

    public static async Task<IResult> Handler([FromRoute] Guid chapterId, [FromRoute] int pageNumber, AppDbContext context, IWebHostEnvironment env)
    {
        ComicPage? page = await context.ComicPages.FindAsync(chapterId, pageNumber);
        if (page is null)
        {
            return Results.NotFound($"Page not found");
        }

        string? basePath = Path.GetDirectoryName(Path.Combine(env.WebRootPath, page.ImageUrl.TrimStart('/')));
        if (Directory.Exists(basePath))
        {
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(page.ImageUrl);
            foreach (string filePath in Directory.GetFiles(basePath, $"{fileNameWithoutExt}*"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        context.ComicPages.Remove(page);
        await context.SaveChangesAsync();

        return Results.Ok();
    }
}
