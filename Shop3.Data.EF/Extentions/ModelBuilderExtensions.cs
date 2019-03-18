
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Data.EF.Extentions
{
    // config lại các thuộc tính của entity ví dụ : int => string
    // là 1 ExtensionsMethod(Extensions phải có static)
    public static class  ModelBuilderExtensions
        {
            public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, // kiểu dữ liệu muốn đặt extensions
                       DbEntityConfiguration<TEntity> entityConfiguration) where TEntity : class
                          {
                                 modelBuilder.Entity<TEntity>(entityConfiguration.Configure);
                          }
        }

        public abstract class DbEntityConfiguration<TEntity> where TEntity : class
        {
            public abstract void Configure(EntityTypeBuilder<TEntity> entity); // đẩy entity vào đẻ config lại
        }
    
}
