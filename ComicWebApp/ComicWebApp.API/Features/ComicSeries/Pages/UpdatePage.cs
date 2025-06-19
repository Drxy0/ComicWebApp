using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace ComicWebApp.API.Features.ComicSeries.Pages;

public class UpdatePage
{
    public record Request(IFormFile ImageFile);
    public class Endpnoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("chapter/{chapterId:guid}/{pageNumber:int}", Handler)
                .WithTags(Tags.Pages);
        }
    }

    public static async Task<IResult> Handler([FromRoute] Guid chapterId, [FromRoute] int pageNumber, [FromForm] Request request, 
        AppDbContext context, IWebHostEnvironment env)
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

        string relativeDirPath = ComicPathHelper.GetRelativeDirPath(page.ImageUrl);
        string fileName = ComicPathHelper.GetChapterFileName(page.PageNumber, request.ImageFile);
        string newRelativeUrl = $"{relativeDirPath}/{fileName}";

        string absoluteFilePath = Path.Combine(env.WebRootPath, newRelativeUrl);

        try
        {
            await using (FileStream stream = File.Create(absoluteFilePath))
            {
                await request.ImageFile.CopyToAsync(stream);
            }

            page.ImageUrl = newRelativeUrl;

            await context.SaveChangesAsync();

            ComicPageResponse response = new ComicPageResponse(
                page.ChapterId,
                page.PageNumber, page.ImageUrl
            );

            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError("Error saving the file");
        }
    }
}
