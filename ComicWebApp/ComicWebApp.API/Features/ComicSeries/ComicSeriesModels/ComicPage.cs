using ComicWebApp.API.Abstractions;
using System.Text.Json.Serialization;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

public class ComicPage : Entity
{
    public int PageNumber { get; set; }
    public string ImageUrl { get; set; }
    public Guid ChapterId { get; set; } // FK to ComicChapter
    public ComicChapter Chapter { get; set; } // navigation prop to traverse back to ComicChapter, idk if I need this
}
