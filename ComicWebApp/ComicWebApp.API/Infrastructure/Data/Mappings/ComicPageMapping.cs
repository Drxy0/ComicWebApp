using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComicWebApp.API.Infrastructure.Data.Mappings;

internal class ComicPageMapping : IEntityTypeConfiguration<ComicPage>
{
    public void Configure(EntityTypeBuilder<ComicPage> builder)
    {
        builder.HasKey(p => new { p.ChapterId, p.PageNumber });

        builder
            .HasOne(p => p.Chapter)
            .WithMany(c => c.Pages)
            .HasForeignKey(p => p.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => r.ChapterId);
    }
}
