using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.ComicSeries.Chapters;

public class UpdateChapter
{
    public record RequestPage(int PageNumber, IFormFile ImageFile);
    public record Request(string? Title, float Number, List<RequestPage> Pages);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("chapter/{id:guid}", Handler)
                .DisableAntiforgery()
                .WithTags("Chapters");
        }
    }

    public static async Task<IResult> Handler(
        [FromRoute] Guid id,
        [FromForm] Request request,
        AppDbContext context,
        IWebHostEnvironment env)
    {
        ComicChapter? chapter = await context.ComicChapters
            .Include(c => c.Pages) // Load existing pages
            .FirstOrDefaultAsync(c => c.Id == id);

        if (chapter is null)
        {
            return Results.NotFound($"Chapter with Id {id} not found");
        }

        ComicSeriesModel? series = await context.ComicSeries.FindAsync(chapter.SeriesId);
        if (series is null)
        {
            return Results.NotFound($"Chapter with Id {id} does not have a related comic series");
        }

        chapter.Title = request.Title;
        chapter.Number = request.Number;

        string relativePath = Path.Combine(
            "ComicSeries",
            ComicPathHelper.GetSeriesFolderName(chapter.Series!),
            ComicPathHelper.GetChapterFolderName(chapter)
        );

        // File Processing Loop
        foreach (RequestPage requestPage in request.Pages.OrderBy(p => p.PageNumber))
        {
            IFormFile imageFile = requestPage.ImageFile;
            if (imageFile is null || imageFile.Length == 0)
                continue;

            string fileName = ComicPathHelper.GetFileName(requestPage.PageNumber, imageFile);
            string imageUrl = ComicPathHelper.GetRelativeImageUrl(relativePath, fileName);
            
            string absoluteFilePath = Path.Combine(env.WebRootPath, imageUrl);

            ComicPage? existingPage = chapter.Pages.FirstOrDefault(p => p.PageNumber == requestPage.PageNumber);

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
                    PageNumber = requestPage.PageNumber,
                    ImageUrl = imageUrl,
                    ChapterId = chapter.Id
                };

                chapter.Pages.Add(newPage);
            }

            using (FileStream stream = File.Create(absoluteFilePath))
            {
                await imageFile.CopyToAsync(stream);
            }
        }

        await context.SaveChangesAsync();

        return Results.Ok(new ChapterResponse(chapter));
    }
}
