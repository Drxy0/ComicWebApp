using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.ComicSeries.Pages;

public class CreatePage
{
    public record Request(Guid ChapterId, int PageNumber, IFormFile ImageFile);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            // QUESTION: "chapter/{chapterId:guid}/page" or "page" and put chapterId in request body

            app.MapPost("/page", Handler)
                .DisableAntiforgery()
                .WithTags("Pages");
        }
    }

    public static async Task<IResult> Handler([FromForm] Request request, AppDbContext context, IWebHostEnvironment env)
    {
        ComicChapter? chapter = await context.ComicChapters
            .Include(c => c.Pages)
            .Include(c => c.Series)
            .FirstOrDefaultAsync(c => c.Id == request.ChapterId);

        if (chapter is null)
        {
            return Results.NotFound($"Chapter with Id {request.ChapterId} not found");
        }

        string relativePath = Path.Combine(
            "ComicSeries",
            ComicPathHelper.GetSeriesFolderName(chapter.Series!),
            ComicPathHelper.GetChapterFolderName(chapter)
        );

        string uploadsPath = Path.Combine(env.WebRootPath, relativePath);

        string fileName = ComicPathHelper.GetImageFileName(
            request.PageNumber,
            Path.GetExtension(request.ImageFile.FileName)
        );

        string absoluteFilePath = Path.Combine(env.WebRootPath, relativePath, fileName);

        string imageUrl = $"/{Path.Combine(relativePath, fileName).Replace("\\", "/")}";

        ComicPage? existingPage = chapter.Pages.Find(p => p.PageNumber == request.PageNumber);
        if (existingPage is not null)
        {
            string oldFilePath = Path.Combine(env.WebRootPath, existingPage.ImageUrl.TrimStart('/'));
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }

            existingPage.ImageUrl = imageUrl;
        }
        else
        {
            ComicPage newPage = new ComicPage
            {
                Id = Guid.NewGuid(),
                ChapterId = request.ChapterId,
                PageNumber = request.PageNumber,
                ImageUrl = imageUrl
            };

            chapter.Pages.Add(newPage);
        }

        using (FileStream stream = File.Create(absoluteFilePath))
        {
            await request.ImageFile.CopyToAsync(stream);
        }

        await context.SaveChangesAsync();

        return Results.Created();
    }

}
