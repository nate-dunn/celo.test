using System;
using System.Collections.Generic;
using System.Text;
using Celo.Test.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Celo.Test.Data.Config
{
    public class ImageConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder
                .HasOne(i => i.User)
                .WithMany(u => u.ProfileImages)
                .HasForeignKey(u => u.UserId);

            // An index on the join
            builder.HasIndex(i => i.UserId);
        }
    }
}
