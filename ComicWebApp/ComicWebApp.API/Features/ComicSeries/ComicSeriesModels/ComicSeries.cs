using ComicWebApp.API.Abstractions;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

public class ComicSeriesModel : Entity
{
    public ComicSeriesMetadata Metadata { get; set; } = new();
    public ComicSeriesAppStats Stats { get; set; } = new();
    public List<ComicChapter> Chapters { get; set; } = new();
    public bool IsVerified { get; set; }
}
