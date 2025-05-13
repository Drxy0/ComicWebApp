using Microsoft.EntityFrameworkCore;
using ComicWebApp.API.Infrastructure.Data.Mappings;
using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using ComicWebApp.API.Features.Users.UserModels;

namespace ComicWebApp.API.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ComicChapter> ComicChapters { get; set; }
    public DbSet<ComicPage> ComicPages { get; set; }
    public DbSet<ComicSeriesModel> ComicSeries { get; set; }
    public DbSet<ComicSeriesAppStats> ComicSeriesAppStats { get; set; }
    public DbSet<ComicSeriesMetadata> ComicSeriesMetadata { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ComicChapterMapping());
        modelBuilder.ApplyConfiguration(new ComicSeriesMapping());
        modelBuilder.ApplyConfiguration(new RefreshTokenMapping());
    }
}
