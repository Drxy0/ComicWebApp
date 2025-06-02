
using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.ComicSeries.Pages;

public class CreatePage
{
    public record Request(IFormFile ImageFile);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("comic/{chapterId:guid}/{pageNumber:int}", Handler)
               .WithTags(Tags.Pages);
        }
    }

    public static async Task<IResult> Handler([FromRoute] Guid chapterId, [FromRoute] int pageNumber, [FromForm] Request request, 
        AppDbContext context, IWebHostEnvironment env)
    {
        ComicChapter? chapter = await context.ComicChapters
            .Include(c => c.Pages)
            .Include(c => c.Series)
            .FirstOrDefaultAsync(c => c.Id == chapterId);

        if (chapter is null)
        {
            return Results.NotFound($"Chapter with Id {chapterId} not found");
        }

        string relativePath = Path.Combine(
            "ComicSeries",
            ComicPathHelper.GetSeriesFolderName(chapter.Series!),
            ComicPathHelper.GetChapterFolderName(chapter)
        );

        string uploadsPath = Path.Combine(env.WebRootPath, relativePath);
        string fileName = ComicPathHelper.GetChapterFileName(pageNumber, request.ImageFile);
        string imageUrl = ComicPathHelper.GetRelativeImageUrl(relativePath, fileName);
        
        string absoluteFilePath = Path.Combine(env.WebRootPath, imageUrl);

        ComicPage? pageToReturn = null;
        ComicPage? existingPage = chapter.Pages.Find(p => p.PageNumber == pageNumber);
        if (existingPage is not null)
        {
            string oldFilePath = Path.Combine(env.WebRootPath, existingPage.ImageUrl.TrimStart('/'));
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }

            existingPage.ImageUrl = imageUrl;
            pageToReturn = existingPage;
        }
        else
        {
            ComicPage newPage = new ComicPage
            {
                ChapterId = chapterId,
                PageNumber = pageNumber,
                ImageUrl = imageUrl
            };

            chapter.Pages.Add(newPage);
            pageToReturn = newPage;
        }

        try
        {
            using (FileStream stream = File.Create(absoluteFilePath))
            {
                await request.ImageFile.CopyToAsync(stream);
            }

            await context.SaveChangesAsync();

            ComicPageResponse response = new ComicPageResponse(
                pageToReturn.ChapterId,
                pageToReturn.PageNumber, pageToReturn.ImageUrl
            );

            return Results.Created($"comic/{chapter.Id}/{pageToReturn.PageNumber}", response);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError("Error saving the file");
        }

    }

}
