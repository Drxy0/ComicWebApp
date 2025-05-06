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
        base.OnModelCreating(modelBuilder);

        // make comicSeries and its metadata share PK
        modelBuilder.Entity<ComicSeries>()
            .HasOne(s => s.Metadata)
            .WithOne(m => m.ComicSeries)
            .HasForeignKey<ComicSeriesMetadata>(m => m.Id);

        modelBuilder.Entity<ComicSeries>()
            .HasOne(s => s.Stats)
            .WithOne(a => a.ComicSeries)
            .HasForeignKey<ComicSeriesAppStats>(a => a.Id);

        modelBuilder.Entity<ComicSeries>()
            .HasMany(s => s.Chapters)
            .WithOne(c => c.Series)
            .HasForeignKey(c => c.SeriesId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ComicChapter>()
            .HasMany(c => c.Pages)
            .WithOne(p => p.Chapter)
            .HasForeignKey(p => p.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ComicPage>()
            .HasIndex(p => p.ChapterId);

        modelBuilder.Entity<ComicChapter>()
            .HasIndex(c => c.SeriesId);
    }
}
