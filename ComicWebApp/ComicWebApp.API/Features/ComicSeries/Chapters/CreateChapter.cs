using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ComicWebApp.API.Features.ComicSeries.Chapters.GetChapter;

namespace ComicWebApp.API.Features.ComicSeries.Chapters;

public class CreateChapter
{
    public record RequestPage(int PageNumber, IFormFile ImageFile);
    public record Request(Guid SeriesId, string? Title, float Number, string Language, List<RequestPage> Pages);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("chapter", Handler)
                .DisableAntiforgery()
                .WithTags(Tags.Chapters);
        }
    }

    public static async Task<IResult> Handler([FromForm] Request request, AppDbContext context, IWebHostEnvironment env)
    {
        ComicSeriesModel? series = await context.ComicSeries
            .Include(s => s.Metadata)
            .FirstOrDefaultAsync(s => s.Id == request.SeriesId);
        if (series is null)
        {
            return Results.NotFound($"Comic series with Id {request.SeriesId} not found");
        }

        if (await context.ComicChapters.AnyAsync(c => c.SeriesId == request.SeriesId && c.Number == request.Number))
        {
            return Results.Conflict($"Chapter {request.Number} already exists in this series");
        }

        ComicChapter chapter = new ComicChapter
        {
            Title = request.Title,
            Number = request.Number,
            Language = request.Language,
            SeriesId = request.SeriesId,
            Pages = new List<ComicPage>()
        };

        context.ComicChapters.Add(chapter);

        await context.SaveChangesAsync();

        try
        {
            string relativePath = Path.Combine(
                "ComicSeries",
                ComicPathHelper.GetSeriesFolderName(chapter.Series!),
                ComicPathHelper.GetChapterFolderName(chapter)
            );

            string uploadsPath = Path.Combine(env.WebRootPath, relativePath);
            Directory.CreateDirectory(uploadsPath);

            // File Processing Loop
            foreach (RequestPage requestPage in request.Pages.OrderBy(p => p.PageNumber))
            {
                IFormFile imageFile = requestPage.ImageFile;
                if (imageFile is null || imageFile.Length == 0)
                    continue;

                string fileName = ComicPathHelper.GetChapterFileName(requestPage.PageNumber, imageFile);

                string absoluteFilePath = Path.Combine(uploadsPath, fileName);

                using (FileStream stream = File.Create(absoluteFilePath))
                {
                    await imageFile.CopyToAsync(stream);
                }

                string imageUrl = $"/{Path.Combine(relativePath, fileName).Replace("\\", "/")}";

                ComicPage page = new ComicPage
                {
                    PageNumber = requestPage.PageNumber,
                    ImageUrl = imageUrl,
                    ChapterId = chapter.Id
                };

                chapter.Pages.Add(page);
            }

            await context.SaveChangesAsync();

            return Results.Created($"chapter/{chapter.Id}", new ChapterResponse(chapter));
        } 
        catch (Exception ex)
        {
            context.ComicChapters.Remove(chapter);
            await context.SaveChangesAsync();

            //TODO: Add proper logging
            Console.WriteLine($"[ERROR] Failed to save uploaded files: {ex.Message}");
            return Results.InternalServerError("Error saving the uploaded files");
        }
    }
}
