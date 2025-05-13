using ComicWebApp.API.Abstractions;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;

namespace ComicWebApp.API.Features.Users.UserModels
{
    public class ComicListEntry : Entity
    {
        public float Rating { get; set; }
        public string? Note { get; set; }
        public bool IsFavorite { get; set; }
        public Guid ReviewId { get; set; }
        public Review? Review { get; set; } // NOTE: Nav props should be nullable
        public Guid ComicSeriesId { get; set; }
        public ComicSeriesModel? ComicSeries { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
