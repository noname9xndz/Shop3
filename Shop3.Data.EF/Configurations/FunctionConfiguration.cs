using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop3.Data.EF.Extentions;
using Shop3.Data.Entities;

namespace Shop3.Data.EF.Configurations
{
    public class FunctionConfiguration : DbEntityConfiguration<Function>
    {
        public override void Configure(EntityTypeBuilder<Function> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).IsRequired()
                .HasMaxLength(128).IsUnicode(false);
            // etc.
        }
    }
}
