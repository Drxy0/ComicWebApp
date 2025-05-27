using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeries.Dtos;

public record ComicSeriesAppStatsDto(
    float Rating,
    int ReviewCount,
    int NumberOfReaders,
    float DropRate
)
{
    public ComicSeriesAppStatsDto(ComicSeriesAppStats stats) : this(
        stats.Rating,
        stats.ReviewCount,
        stats.NumberOfReaders,
        stats.DropRate)
    {
    }
}