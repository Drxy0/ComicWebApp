using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

    public static string GetChapterFileName(int pageNumber, IFormFile imageFile)
    {
        return $"{pageNumber}{Path.GetExtension(imageFile.FileName).ToLower()}";
    }
    public static string GetRelativeDirPath(string relativeImageUrl)
    {
        return relativeImageUrl[..relativeImageUrl.LastIndexOf('/')];
    }
    public static string GetRelativeImageUrl(string relativePath, string fileName)
    {
        return $"/{Path.Combine(relativePath.Trim('/'), fileName).Replace("\\", "/")}";
    }
}
