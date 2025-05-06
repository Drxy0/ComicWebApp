using ComicWebApp.DAL.Mappings;
using ComicWebApp.DAL.Models.ComicSeriesModels;
using ComicWebApp.DAL.Models.User;
using Microsoft.EntityFrameworkCore;

namespace ComicWebApp.DAL.Context;

public class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ComicChapter> ComicChapters { get; set; }
    public DbSet<ComicPage> ComicPages { get; set; }
    public DbSet<ComicSeries> ComicSeries { get; set; }
    public DbSet<ComicSeriesAppStats> ComicSeriesAppStats { get; set; }
    public DbSet<ComicSeriesMetadata> ComicSeriesMetadata { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ComicChapterMapping());
        modelBuilder.ApplyConfiguration(new ComicSeriesMapping());
        modelBuilder.ApplyConfiguration(new RefreshTokenMapping());
    }
}
