using ComicWebApp.DAL.Models.Abstractions;
using ComicWebApp.DAL.Models.ComicSeriesModels;

namespace ComicWebApp.DAL.Models.User
{
    public class ComicListEntry : Entity
    {
        public float Rating { get; set; }
        public string? Note { get; set; }
        public bool IsFavorite { get; set; }
        public Guid ReviewId { get; set; }
        public Review? Review { get; set; } // NOTE: Nav props should be nullable
        public Guid ComicSeriesId { get; set; }
        public ComicSeries? ComicSeries { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
