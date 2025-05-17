using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

namespace ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;

public record ComicSeriesAppStatsDto(
    Guid Id,
    float Rating,
    int ReviewCount,
    int NumberOfReaders,
    float CompletionRate,
    float DropRate
)
{
    public ComicSeriesAppStatsDto(ComicSeriesAppStats stats) : this(
        stats.Id,
        stats.Rating,
        stats.ReviewCount,
        stats.NumberOfReaders,
        stats.CompletionRate,
        stats.DropRate)
    {
    }
}