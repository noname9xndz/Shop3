using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop3.Data.EF.Extentions;
using Shop3.Data.Entities;

namespace Shop3.Data.EF.Configurations
{
    public class AdvertistmentPositionConfiguration : DbEntityConfiguration<AdvertistmentPosition>
    {
        public override void Configure(EntityTypeBuilder<AdvertistmentPosition> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(20).IsRequired();
            // etc.
        }
    }
}
