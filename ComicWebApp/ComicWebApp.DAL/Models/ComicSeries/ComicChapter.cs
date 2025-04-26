namespace ComicWebApp.DAL.Models.ComicSeries;

public class ComicChapter
{
    public string? Title { get; set; }
    public float Number { get; set; } // float because of half chapters e.g. 40, 40.5, 41
    public List<ComicPage> Pages { get; set; } = new();
}
