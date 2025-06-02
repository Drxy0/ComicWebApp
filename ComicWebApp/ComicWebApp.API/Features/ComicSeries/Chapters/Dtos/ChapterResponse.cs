using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

namespace ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;

public record ChapterResponse(string? Title, float Number, string Language, Guid Id, Guid SeriesId, int PageCount)
{
    public ChapterResponse(ComicChapter chapter)
        : this(
            chapter.Title,
            chapter.Number,
            chapter.Language,
            chapter.Id,
            chapter.SeriesId,
            chapter.Pages.Count()
        )
    { 
    }
}
