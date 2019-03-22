using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop3.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop3.Data.EF;
using Shop3.Data.Entities;
using AutoMapper;
using Shop3.Application.Interfaces;
using Shop3.Application.Implementation;
using Shop3.Application.AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Shop3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });



            #region lấy connection từ Configuration để migration

            services.AddDbContext<AppDbContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                  o => o.MigrationsAssembly("Shop3.Data.EF"))); // add thêm options ko dùng project hiện tại mà dùng  Shop3.Data.EF

            #endregion

            #region add identity + config identity

            //services.AddIdentity<AppUser,AppRole>() // sử dụng AppUser và AppRole tự tao ko dùng mặc định của Identity
            //  .AddDefaultUI(UIFramework.Bootstrap4)
            //  .AddEntityFrameworkStores<AppDbContext>();

            services.AddIdentity<AppUser, AppRole>()
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();

            // Configure cơ chế  mặc định register user của Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            #endregion

            #region cofig cho  auto mapper
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });

            #endregion

            #region Add application services

            services.AddAutoMapper();  // nuget : AutoMapper.Extensions.Microsoft.DependencyInjection

            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>(); //khai báo khởi tạo thông tin user, và role
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>(); //AddScoped giới hạn 1 request gửi lên

            services.AddSingleton(Mapper.Configuration); // nuget : automapper
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));



            services.AddTransient<DbInitializer>(); // gọi DbInitializer lúc khởi tạo


            //Register Serrvices
            services.AddTransient<IProductCategoryService, ProductCategoryService>();

            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                // config lại object trả về khi respone trả về cho ajax
                .AddJsonOptions( 
                      options => options.SerializerSettings.ContractResolver = new  DefaultContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/noname-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // register area
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
            });

            
        }
    }
}
