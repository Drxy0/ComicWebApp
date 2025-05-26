using ComicWebApp.API.Abstractions;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels.Enums;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

public class ComicSeriesMetadata : Entity
{
    // searching by author(s)' name is out of scope, so save as string
    // for manga/general
    public string Title { get; set; }
    public string? Author { get; set; }
    public string? Artist { get; set; }
    public int? YearOfRelease { get; set; }

    // for comics
    public string? Writer { get; set; }
    public string? Penciler { get; set; }
    public string? Inker { get; set; }
    public string? Colorist { get; set; }

    public string? CoverImageUrl { get; set; }
    public string? Description { get; set; }
    public string? OriginalLanguage { get; set; }
    public PublicationStatus PublicationStatus { get; set; }
    public List<Genre> Genres { get; set; }
    public List<Theme> Themes { get; set; }
    public ComicSeriesModel? ComicSeries { get; set; } // FK

}
