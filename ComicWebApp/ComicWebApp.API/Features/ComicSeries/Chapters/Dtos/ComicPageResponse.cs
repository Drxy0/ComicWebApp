namespace ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;

public record ComicPageResponse(Guid ChapterId, int PageNumber, string ImageUrl);