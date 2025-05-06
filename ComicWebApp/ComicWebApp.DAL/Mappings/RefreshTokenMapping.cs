using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicWebApp.DAL.Mappings
{
    internal class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Token).HasMaxLength(200);
            builder.HasIndex(r => r.Token).IsUnique();
            builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId);
        }
    }
}
