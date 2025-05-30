namespace ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;

public record ComicPageResponse(Guid Id, Guid ChapterId, int PageNumber, string ImageUrl);