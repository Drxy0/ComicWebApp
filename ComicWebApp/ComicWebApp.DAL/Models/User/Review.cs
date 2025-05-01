using ComicWebApp.DAL.Models.Abstractions;
using ComicWebApp.Shared.Enums;

namespace ComicWebApp.DAL.Models.User;

public class Review : Entity
{
    public ReviewType GeneralOpinion { get; set; }
    public string? Description { get; set; }
    // TODO: was this review helpful? section
}
