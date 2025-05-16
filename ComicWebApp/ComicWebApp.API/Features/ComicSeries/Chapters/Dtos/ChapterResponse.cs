namespace ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;

public record ChapterFilesResponse(Guid Id, int PageNumber);
public record ChapterResponse(string? Title, float Number, Guid Id, Guid SeriesId, List<ChapterFilesResponse> Pages);
