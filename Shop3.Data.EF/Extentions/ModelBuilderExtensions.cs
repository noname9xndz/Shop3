
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Data.EF.Extentions
{
    // config lại các thuộc tính của entity ví dụ : int => string
        public static class  ModelBuilderExtensions
        {
            public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder,
                       DbEntityConfiguration<TEntity> entityConfiguration) where TEntity : class
                          {
                                 modelBuilder.Entity<TEntity>(entityConfiguration.Configure);
                          }
        }

        public abstract class DbEntityConfiguration<TEntity> where TEntity : class
        {
            public abstract void Configure(EntityTypeBuilder<TEntity> entity);
        }
    
}
