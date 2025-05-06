using ComicWebApp.DAL.Models.Abstractions;
using ComicWebApp.DAL.Models.ComicSeriesModels;

namespace ComicWebApp.DAL.Models.User
{
    public class User : Entity
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public List<User> Friends { get; set; } = new();
        public List<ComicListEntry> ComicsList { get; set; } = new();
        //public List<ComicSeries> Favourites { get; set; } = new();
        public bool IsAdmin { get; set; }
    }
}
