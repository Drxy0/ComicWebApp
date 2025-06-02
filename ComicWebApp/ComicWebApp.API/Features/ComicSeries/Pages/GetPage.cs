using ComicWebApp.API.Endpoints;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.API.Features.ComicSeries.Pages;

public class GetPage
{

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("comic/{chapterId:guid}/{pageNumber:int}", Handler)
                .WithTags(Tags.Pages);
        }
    }

    public static async Task<IResult> Handler([FromRoute] Guid chapterId, [FromRoute] int pageNumber, 
        AppDbContext context, IWebHostEnvironment env)
    {
        ComicPage? page = await context.ComicPages
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                p.ChapterId == chapterId &&
                p.PageNumber == pageNumber);

        if (page is null)
        {
            return Results.NotFound($"Page not found");
        }

        string imagePath = Path.Combine(
            env.WebRootPath,
            page.ImageUrl.TrimStart('/'));

        if (!File.Exists(imagePath))
        {
            return Results.InternalServerError("Page doesn't have an associated file");
        }


        // TODO: Make it so images convert to webp in CreateChapter
        // not every time client calls getPage (save multiple copies for multiple compression rates)
        try
        {
            MemoryStream? memoryStream = await WebpConverter.ConvertToWebpAsync(imagePath, 70, 0.5f)!;

            if (memoryStream is null)
            {
                return Results.InternalServerError("Error processing image");
            }

            return Results.File(
                memoryStream,
                "image/webp",
                $"{page.PageNumber}.webp");
        }
        catch(ArgumentOutOfRangeException ex)
        {
            // QUESTION: How to handle this case
            Console.WriteLine(ex.Message);
            return Results.InternalServerError("Programmer is dumb dumb");
        }
    }
}
