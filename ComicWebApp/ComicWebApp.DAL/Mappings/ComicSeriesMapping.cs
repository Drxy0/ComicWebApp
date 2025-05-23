﻿using ComicWebApp.DAL.Models.ComicSeriesModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ComicWebApp.DAL.Mappings
{
    internal class ComicSeriesMapping : IEntityTypeConfiguration<ComicSeries>
    {
        public void Configure(EntityTypeBuilder<ComicSeries> builder)
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
}
