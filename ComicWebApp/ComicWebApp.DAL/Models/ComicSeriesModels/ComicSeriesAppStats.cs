using ComicWebApp.DAL.Models.Abstractions;

namespace ComicWebApp.DAL.Models.ComicSeriesModels;

public class ComicSeriesAppStats : Entity
{
    public float Rating { get; set; }
    public int ReviewCount { get; set; } // int is the most supported type in databases, so uint, short or ushort wont work
    public int NumberOfReaders { get; set; }
    public float CompletionRate { get; set; }  // Percentage of readers who finished the comic
    public float DropRate { get; set; }  // Percentage of readers who dropped it

    public ComicSeries ComicSeries { get; set; } // FK
}
