using ComicWebApp.Shared.Enums;

namespace ComicWebApp.DAL.Models.ComicSeries;

public class ComicSeriesMetadata
{
    // searching by author(s)' name is out of scope, so save as string
    // for manga/general
    public string? Author { get; set; }
    public string? Artist { get; set; }
    
    // for comics
    public string? Writer { get; set; }
    public string? Penciler { get; set; }
    public string? Inker { get; set; }
    public string? Colorist { get; set; }
    
    public string? Description { get; set; }
    public string? OriginalLanguage { get; set; }
    public PublicationStatus PublicationStatus { get; set; }
    public List<Genre> Genres { get; set; } = new();
    public List<Theme> Themes { get; set; } = new();

}
