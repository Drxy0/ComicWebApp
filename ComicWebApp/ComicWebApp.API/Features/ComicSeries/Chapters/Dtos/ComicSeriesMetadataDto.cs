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
);