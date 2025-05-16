using ComicWebApp.API.Abstractions;
using System.Text.Json.Serialization;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

public class ComicSeriesModel : Entity
{
    public ComicSeriesMetadata Metadata { get; set; }
    public ComicSeriesAppStats Stats { get; set; }
    public List<ComicChapter> Chapters { get; set; } = new();
    public bool IsVerified { get; set; }
}
