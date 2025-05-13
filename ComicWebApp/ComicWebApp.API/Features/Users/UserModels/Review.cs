using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels.Enums;
using ComicWebApp.API.Models.Abstractions;

namespace ComicWebApp.API.Features.Users.UserModels;

public class Review : Entity
{
    public ReviewType GeneralOpinion { get; set; }
    public string? Description { get; set; }
    // TODO: was this review helpful? section
}
