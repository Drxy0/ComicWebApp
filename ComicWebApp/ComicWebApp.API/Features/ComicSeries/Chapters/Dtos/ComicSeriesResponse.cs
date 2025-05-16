namespace ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;

public record ComicSeriesResponse(
    Guid Id, 
    ComicSeriesMetadataDto Metadata,
    ComicSeriesAppStatsDto? Stats,
    List<ChapterResponse>? Chapters, 
    bool IsVerified
);

