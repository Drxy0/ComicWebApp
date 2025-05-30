using ComicWebApp.API.Abstractions;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels.Enums;

namespace ComicWebApp.API.Features.Users.UserModels;

public class Review : Entity
{
    public ReviewType GeneralOpinion { get; set; }
    public string? Description { get; set; }
    // TODO: was this review helpful? section
}
