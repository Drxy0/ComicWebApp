using ComicWebApp.API.Abstractions;
using System.Text.Json.Serialization;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

public class ComicChapter : Entity
{
    public string? Title { get; set; }
    public float Number { get; set; } // float because of half chapters e.g. 40, 40.5, 41
    public List<ComicPage> Pages { get; set; } = new();

    public Guid SeriesId { get; set; }
    [JsonIgnore]
    public ComicSeriesModel? Series { get; set; }
}
