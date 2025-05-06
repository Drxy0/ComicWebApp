using ComicWebApp.DAL.Models.ComicSeriesModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComicWebApp.DAL.Mappings;

internal class ComicPageMapping : IEntityTypeConfiguration<ComicPage>
{
    public void Configure(EntityTypeBuilder<ComicPage> builder)
    {
        builder
            .HasOne(p => p.Chapter)
            .WithMany(c => c.Pages)
            .HasForeignKey(p => p.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => r.ChapterId);
    }
}
