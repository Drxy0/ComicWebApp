namespace ComicWebApp.DAL.Models.ComicSeries;

public class ComicSeries
{
    public ComicSeriesMetadata Metadata { get; set; } = new();
    public List<ComicChapter> Chapters { get; set; } = new();
    public bool IsVerified { get; set; }

}
