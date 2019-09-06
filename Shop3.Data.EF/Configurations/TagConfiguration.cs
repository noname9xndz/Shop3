using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop3.Data.EF.Extentions;
using Shop3.Data.Entities;

namespace Shop3.Data.EF.Configurations
{
    // nhớ add các thư viện trên nuget 
    public class TagConfiguration : DbEntityConfiguration<Tag>
    {
        // config lại thuộc tính của đối tượng Id trong bảng Tag về dạng varchar("50")
        public override void Configure(EntityTypeBuilder<Tag> entity)
        {

            entity.Property(c => c.Id).HasMaxLength(50).IsRequired().IsUnicode(false).HasMaxLength(50);

        }
    }
}
