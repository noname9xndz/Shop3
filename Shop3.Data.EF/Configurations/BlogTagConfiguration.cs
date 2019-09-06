using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop3.Data.EF.Extentions;
using Shop3.Data.Entities;

namespace Shop3.Data.EF.Configurations
{
    public class BlogTagConfiguration : DbEntityConfiguration<BlogTag>
    {
        public override void Configure(EntityTypeBuilder<BlogTag> entity)
        {
            entity.Property(c => c.TagId).HasMaxLength(50).IsRequired()
            .IsUnicode(false).HasMaxLength(50);
            // etc.
        }
    }
}
