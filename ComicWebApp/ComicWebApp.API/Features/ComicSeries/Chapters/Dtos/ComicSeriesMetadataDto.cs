using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels.Enums;

namespace ComicWebApp.API.Features.ComicSeries.Chapters.Dtos;

public record ComicSeriesMetadataDto(
    Guid Id,
    string Title,
    string? Author,
    string? Artist,
    int? YearOfRelease,
    string? Writer,
    string? Penciler,
    string? Inker,
    string? Colorist,
    string? Description,
    string? OriginalLanguage,
    PublicationStatus PublicationStatus,
    List<Genre> Genres,
    List<Theme> Themes
)
{
    public ComicSeriesMetadataDto(ComicSeriesMetadata metadata) : this(
        metadata.Id,
        metadata.Title,
        metadata.Author,
        metadata.Artist,
        metadata.YearOfRelease,
        metadata.Writer,
        metadata.Penciler,
        metadata.Inker,
        metadata.Colorist,
        metadata.Description,
        metadata.OriginalLanguage,
        metadata.PublicationStatus,
        metadata.Genres ?? new List<Genre>(),
        metadata.Themes ?? new List<Theme>())
    {
    }
}