using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop3.Data.EF.Extentions;
using Shop3.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static Shop3.Data.EF.Extentions.ModelBuilderExtensions;

namespace Shop3.Data.EF.Configurations
{
   // nhớ add các thư viện trên nuget 
    public class TagConfiguration : DbEntityConfiguration<Tag>
    {
        // config lại thuộc tính của đối tượng Id trong bảng Tag về dạng varchar("50")
        public override void Configure(EntityTypeBuilder<Tag> entity)
        {
            // 1 trong 2 cách
            entity.Property(c => c.Id).HasMaxLength(50).IsRequired().IsUnicode(false).HasMaxLength(50);

          // hoặc  entity.Property(c => c.Id).HasMaxLength(50).IsRequired().HasColumnType("varchar(50)");
        }
    }
}
