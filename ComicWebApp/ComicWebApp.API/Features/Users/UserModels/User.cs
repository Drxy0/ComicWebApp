using ComicWebApp.API.Abstractions;

namespace ComicWebApp.API.Features.Users.UserModels
{
    public class User : Entity
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public List<User> Friends { get; set; } = new();
        public List<ComicListEntry> ComicsList { get; set; } = new();
        public bool IsAdmin { get; set; }
    }
}
