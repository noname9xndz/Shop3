using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Shop3.Application.AutoMapper;
using Shop3.Application.Implementation;
using Shop3.Application.Interfaces;
using Shop3.Authorization;
using Shop3.Data.EF;
using Shop3.Data.Entities;
using Shop3.Helpers;
using Shop3.Infrastructure.Interfaces;
using Shop3.Services;
using System;

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
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            #region lấy connection từ Configuration để migration

            services.AddDbContext<AppDbContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                  o => o.MigrationsAssembly("Shop3.Data.EF"))); // add thêm options ko dùng project hiện tại mà dùng  Shop3.Data.EF

            #endregion lấy connection từ Configuration để migration

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

            #endregion add identity + config identity

            #region config cho  auto mapper ,captcha 

            // https://github.com/PaulMiami/reCAPTCHA/wiki/Getting-started , config key appsetting
            // https://www.google.com/recaptcha/admin register key
            services.AddRecaptcha(new RecaptchaOptions()
            {
                SiteKey = Configuration["Recaptcha:SiteKey"],
                SecretKey = Configuration["Recaptcha:SecretKey"]
            });

           
           

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });


            #endregion cofig cho  auto mapper

            #region Add application services

            // nuget Microsoft.AspNetCore.Session
            services.AddSession(options =>
            {
                //options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true; //1 cookies có được truy cập bởi client script ko
            });

            services.AddAutoMapper();  // nuget : AutoMapper.Extensions.Microsoft.DependencyInjection

            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>(); //khai báo khởi tạo thông tin user, và role
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>(); //AddScoped giới hạn 1 request gửi lên

            services.AddSingleton(Mapper.Configuration); // nuget : automapper
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            services.AddTransient<IEmailSender,EmailSender>(); // send mail to user
            services.AddTransient<IViewRenderService, ViewRenderService>(); // send bill mail  to user

            services.AddTransient<DbInitializer>(); // gọi DbInitializer lúc khởi tạo
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>(); // register cơ chế ghi đè ClaimsPrincipal

            //services.AddTransient<IFunctionRepository, FunctionRepository>(); register Repository nếu dùng

            //Register Serrvices
            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));

            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<IFunctionService, FunctionService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IBillService, BillService>();
            services.AddTransient<ICommonService,CommonService>();
            services.AddTransient<IBlogService, BlogService>();

            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();

            #endregion Add application services

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                // config lại object trả về khi respone trả về cho ajax
                .AddJsonOptions(
                      options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/noname-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
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
            app.UseSession(); //nuget Microsoft.AspNetCore.Session
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