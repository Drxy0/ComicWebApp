using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

namespace ComicWebApp.API.Features.ComicSeries;

public static class ComicPathHelper
{
    public static string GetSeriesFolderName(ComicSeriesModel series)
    {
        return $"{series.Metadata.Title} - {series.Id.ToString()[..8]}";
    }

    public static string GetChapterFolderName(ComicChapter chapter)
    {
        return $"{chapter.Number} - {chapter.Title} - {chapter.Id.ToString()[..8]}";
    }

    public static string GetImageFileName(int pageNumber, string fileExtension)
    {
        return $"{pageNumber}{fileExtension.ToLower()}";
    }
}
