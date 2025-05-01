using ComicWebApp.DAL.Models.Abstractions;

namespace ComicWebApp.DAL.Models.ComicSeriesModels;

public class ComicSeries : Entity
{
    public ComicSeriesMetadata Metadata { get; set; } = new();
    public ComicSeriesAppStats Stats { get; set; } = new();
    public List<ComicChapter> Chapters { get; set; } = new();
    public bool IsVerified { get; set; }

}
