using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComicWebApp.API.Infrastructure.Data.Mappings;

internal class ComicSeriesMapping : IEntityTypeConfiguration<ComicSeriesModel>
{
    public void Configure(EntityTypeBuilder<ComicSeriesModel> builder)
    {
        // make comicSeries and its metadata share PK
        builder.HasOne(s => s.Metadata)
            .WithOne(m => m.ComicSeries)
            .HasForeignKey<ComicSeriesMetadata>(m => m.Id);

        builder.HasOne(s => s.Stats)
            .WithOne(a => a.ComicSeries)
            .HasForeignKey<ComicSeriesAppStats>(a => a.Id);

        builder.HasMany(s => s.Chapters)
            .WithOne(c => c.Series)
            .HasForeignKey(c => c.SeriesId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
