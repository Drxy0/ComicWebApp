namespace ComicWebApp.DAL.Models.ComicSeries;

public class ComicSeriesAppStats
{
    public float Rating { get; set; }
    public int ReviewCount { get; set; } // int is the most supported type in databases, so uint, short or ushort wont work
    public int NumberOfReaders { get; set; }
    public float CompletionRate { get; set; }  // Percentage of readers who finished the comic
    public float DropRate { get; set; }  // Percentage of readers who dropped it
}
