using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shop3.Data.EF.Configurations;
using Shop3.Data.EF.Extentions;
using Shop3.Data.Entities;
using Shop3.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shop3.Data.EF
{
    // add Microsoft.AspNetCore.Identity và Microsoft.AspNetCore.Identity.entityframeworkcore
    //thay vì add DbConText chúng ta sẽ cho kế thừa từ IdentityDbContext<> để tích hợp ASP.NET Identity

    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid> // key để kiểu guid
    {// objectContext đại diện cho 1 database có chức năng quản lý các kết nối, định nghĩa mô hình dữ liệu với metadata và các thao tác với database
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        // ObjectSet<TEntity> là 1 tập hợp các entity mỗi đối tượng ứng với 1 table,có thể lấy các đối tượng này thông qua property tương ứng của objectContext
        public DbSet<Language> Languages { set; get; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<Function> Functions { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Announcement> Announcements { set; get; }
        public DbSet<AnnouncementUser> AnnouncementUsers { set; get; }

        public DbSet<Blog> Bills { set; get; }
        public DbSet<BillDetail> BillDetails { set; get; }
        public DbSet<Blog> Blogs { set; get; }
        public DbSet<BlogTag> BlogTags { set; get; }
        public DbSet<Color> Colors { set; get; }
        public DbSet<Contact> Contacts { set; get; }
        public DbSet<Feedback> Feedbacks { set; get; }
        public DbSet<Footer> Footers { set; get; }
        public DbSet<Page> Pages { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }
        public DbSet<ProductQuantity> ProductQuantities { set; get; }
        public DbSet<ProductTag> ProductTags { set; get; }

        public DbSet<Size> Sizes { set; get; }
        public DbSet<Slide> Slides { set; get; }

        public DbSet<Tag> Tags { set; get; }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<WholePrice> WholePrices { get; set; }

        public DbSet<AdvertistmentPage> AdvertistmentPages { get; set; }
        public DbSet<Advertistment> Advertistments { get; set; }
        public DbSet<AdvertistmentPosition> AdvertistmentPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // câu hình key cho Identity,ToTable("Tên bảng") : đính nghĩa tên bảng trong sql thay vì dùng [Table("tên bảng")] trong mỗi class

            #region Identity Config

            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims").HasKey(x => x.Id);

            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.RoleId, x.UserId });

            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => new { x.UserId });

            #endregion Identity Config

            // gọi riêng các thằng  đã được cofig lại thuộc tính
            builder.AddConfiguration(new TagConfiguration());
            builder.AddConfiguration(new BlogTagConfiguration());
            builder.AddConfiguration(new ContactDetailConfiguration());
            builder.AddConfiguration(new FooterConfiguration());
            builder.AddConfiguration(new PageConfiguration());
            builder.AddConfiguration(new FooterConfiguration());
            builder.AddConfiguration(new ProductTagConfiguration());
            builder.AddConfiguration(new SystemConfigConfiguration());
            builder.AddConfiguration(new AdvertistmentPositionConfiguration());

            // fix lỗi length id không trùng => comment lại để OnModelCreating ko chạy creating model mặc định mà ghi đè theo cách của ta
            base.OnModelCreating(builder); 
        }
        // ghi đè phương thức SaveChanges để mỗi lần save thằng nào kế thừa IDateTracking sẽ tự động update DateCreated và DateModified 
        public override int SaveChanges()
        {
            // lấy ra những đối tượng thay đổi hoặc được thêm mới
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (EntityEntry item in modified)
            {
                var changedOrAddedItem = item.Entity as IDateTracking;

                if (changedOrAddedItem != null)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.DateCreated = DateTime.Now;
                    }
                    changedOrAddedItem.DateModified = DateTime.Now;
                }
            }
            return base.SaveChanges(); // todo bug loop
        }

       
    }

    // https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/new-db?tabs=visual-studio#create-the-database
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
               
                .SetBasePath(Directory.GetCurrentDirectory()) // nuget : Microsoft.Extensions.configuration.FileExtensions
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // nuget : Microsoft.Extensions.configuration.Json
                .AddEnvironmentVariables()   // requires Microsoft.Extensions.Configuration.EnvironmentVariable
                .Build(); 
                

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection"); 
            builder.UseSqlServer(connectionString);

            return new AppDbContext(builder.Options);
        }
    }


}

