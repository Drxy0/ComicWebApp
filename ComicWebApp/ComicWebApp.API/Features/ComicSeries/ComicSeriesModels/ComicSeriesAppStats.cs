using ComicWebApp.API.Abstractions;

namespace ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

public class ComicSeriesAppStats : Entity
{
    public float Rating { get; set; }
    public int ReviewCount { get; set; } // int is the most supported type in databases, so uint, short or ushort wont work
    public int NumberOfReaders { get; set; }
    public float CompletionRate { get; set; }  // Percentage of readers who finished the comic
    public float DropRate { get; set; }  // Percentage of readers who dropped it
    public ComicSeriesModel? ComicSeries { get; set; } // FK
}
