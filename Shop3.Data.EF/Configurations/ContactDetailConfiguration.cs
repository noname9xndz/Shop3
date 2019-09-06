using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop3.Data.EF.Extentions;
using Shop3.Data.Entities;

namespace Shop3.Data.EF.Configurations
{
    public class ContactDetailConfiguration : DbEntityConfiguration<Contact>
    {
        public override void Configure(EntityTypeBuilder<Contact> entity)
        {
            entity.HasKey(c => c.Id); // cấu hình cho key
            entity.Property(c => c.Id).HasMaxLength(255).IsRequired(); // cấu hình cho thuộc tính của key
            // etc.
        }
    }
}
