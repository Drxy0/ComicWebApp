using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

namespace ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;

public record ChapterFilesResponse(Guid Id, int PageNumber);

public record ChapterResponse(string? Title, float Number, Guid Id, Guid SeriesId, List<ChapterFilesResponse> Pages)
{
    public ChapterResponse(ComicChapter chapter)
        : this(
            chapter.Title,
            chapter.Number,
            chapter.Id,
            chapter.SeriesId,
            chapter.Pages
                .OrderBy(p => p.PageNumber)
                .Select(p => new ChapterFilesResponse(p.Id, p.PageNumber))
                .ToList()
        )
    { 
    }
}
