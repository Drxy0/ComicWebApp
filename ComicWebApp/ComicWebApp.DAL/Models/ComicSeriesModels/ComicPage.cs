using ComicWebApp.DAL.Models.Abstractions;

namespace ComicWebApp.DAL.Models.ComicSeriesModels;

public class ComicPage : Entity
{
    public int PageNumber { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public Guid ChapterId { get; set; } // FK to ComicChapter
    public ComicChapter Chapter { get; set; } // navigation prop to traverse back to ComicChapter, idk if I need this
}
