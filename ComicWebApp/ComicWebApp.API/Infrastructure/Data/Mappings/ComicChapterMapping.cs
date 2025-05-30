﻿using ComicWebApp.API.Features.ComicSeries.ComicSeriesModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComicWebApp.API.Infrastructure.Data.Mappings;

internal class ComicChapterMapping : IEntityTypeConfiguration<ComicChapter>
{
    public void Configure(EntityTypeBuilder<ComicChapter> builder)
    {
        builder.HasMany(c => c.Pages)
            .WithOne(p => p.Chapter)
            .HasForeignKey(p => p.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.SeriesId);

        builder.Property(c => c.Language).HasMaxLength(2);
    }
}
