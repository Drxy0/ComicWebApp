using ComicWebApp.DAL.Models.ComicSeriesModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace ComicWebApp.DAL.Mappings;

internal class ComicChapterMapping : IEntityTypeConfiguration<ComicChapter>
{
    public void Configure(EntityTypeBuilder<ComicChapter> builder)
    {
        builder.HasMany(c => c.Pages)
            .WithOne(p => p.Chapter)
            .HasForeignKey(p => p.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.SeriesId);
    }
}
