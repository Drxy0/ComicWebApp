namespace ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;

public record ComicSeriesAppStatsDto(
    Guid Id,
    float Rating,
    int ReviewCount,
    int NumberOfReaders,
    float CompletionRate,
    float DropRate
);