using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Shop3.Application.AutoMapper;
using Shop3.Application.Dapper.Implementation;
using Shop3.Application.Dapper.SqlCommands;
using Shop3.Application.Implementation;
using Shop3.Application.Interfaces;
using Shop3.Authorization;
using Shop3.Data.EF;
using Shop3.Data.Entities;
using Shop3.Extensions;
using Shop3.Helpers;
using Shop3.Infrastructure.Interfaces;
using Shop3.Middleware;
using Shop3.Services;
using Shop3.SignalR;
using System;
using System.Collections.Generic;
using System.Globalization;

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

            #region add identity + config identity,caching app,minifile

            //services.AddIdentity<AppUser,AppRole>() // sử dụng AppUser và AppRole tự tao ko dùng mặc định của Identity
            //  .AddDefaultUI(UIFramework.Bootstrap4)
            //  .AddEntityFrameworkStores<AppDbContext>();

            services.AddIdentity<AppUser, AppRole>()
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();

            // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-2.2
            // thảm khảo thêm  : https://viblo.asia/p/lam-viec-voi-distributed-cache-trong-aspnet-core-m68Z0O69KkG
            // cache output bằng HTTP-based response caching , cache dữ liệu : bằng distributed hoặc memory
            services.AddMemoryCache(); // phù hợp với các ứng dụng vừa và nhỏ sử dụng 1 server

            //services.AddMinResponse(); // sử dụng middleware WebMarkupMin để nén file

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

            #endregion add identity + config identity,caching app,minifile

            #region config cho  auto mapper ,captcha

            // https://github.com/PaulMiami/reCAPTCHA/wiki/Getting-started , config key appsetting
            // https://www.google.com/recaptcha/admin register key
            services.AddRecaptcha(new RecaptchaOptions()
            {
                SiteKey = Configuration["Recaptcha:SiteKey"],
                SecretKey = Configuration["Recaptcha:SecretKey"]
            });

            //nuget : automapper ,AutoMapper.Extensions.Microsoft.DependencyInjection
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });

            #endregion config cho  auto mapper ,captcha

            #region Add application services

            // nuget Microsoft.AspNetCore.Session
            services.AddSession(options =>
            {
                //options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true; //1 cookies có được truy cập bởi client script ko
            });

            services.AddImageResizer(); // extension using ImageResizerMiddleware để crop ảnh tối ưu hóa load trang  : https://www.paddo.org/asp-net-core-image-resizing-middleware/
            services.AddAutoMapper();  // nuget : AutoMapper.Extensions.Microsoft.DependencyInjection
            services.AddAuthentication() // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/?view=aspnetcore-2.2&tabs=visual-studio
                .AddFacebook(facebookOpts =>
                {
                    // config login bằng fb ,gg
                    // tham khảo  : https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/other-logins?view=aspnetcore-2.2
                    facebookOpts.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookOpts.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(googleOpts =>
                {
                    googleOpts.ClientId = Configuration["Authentication:Google:ClientId"];
                    googleOpts.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });

            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>(); //khai báo khởi tạo thông tin user, và role
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>(); //AddScoped giới hạn 1 request gửi lên

            //services.AddSingleton(Mapper.Configuration); // nuget : automapper
            // services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            services.AddTransient<IEmailSender, EmailSender>(); // send mail to user
            services.AddTransient<IViewRenderService, ViewRenderService>(); // send bill mail  to user
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IExcelService, ExcelService>();

            services.AddTransient<DbInitializer>(); // gọi DbInitializer lúc khởi tạo chạy seed()

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
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IPageService, PageService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IAnnouncementService, AnnouncementService>();
            services.AddTransient<IPageDefaultService, PageDefaultService>();

            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();

            #endregion Add application servicess

            services.AddMvc(options =>
            { // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/response?view=aspnetcore-2.2
              //  response caching bản chất là viết lại các response header để trình duyệt hiểu được cache trong bao lâu
              // cache output bằng HTTP-based response caching , cache dữ liệu : bằng distributed hoặc memory
                options.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 60
                    });
                options.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            })
             .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
             // đa ngôn ngữ bằng CookieRequestCultureProvider nuget : Microsoft.AspNetCore.Mvc.Localization
             .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = "Resources"; })
             .AddDataAnnotationsLocalization() // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2
                                               // config lại object trả về khi respone trả về cho ajax
             .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; }); // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                { // config SignalR
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("https://localhost:44385") // chỉ chop phép domain của ta truy cập
                        .AllowCredentials();
                }));

            services.Configure<RequestLocalizationOptions>(
              opts =>
              {
                  var supportedCultures = new List<CultureInfo>
                  {
                        new CultureInfo("en-US"),
                        new CultureInfo("vi-VN")
                  };

                  opts.DefaultRequestCulture = new RequestCulture("en-US"); // mặc định English
                  // Formatting numbers, dates, etc.
                  opts.SupportedCultures = supportedCultures;
                  // UI strings that we have localized.
                  opts.SupportedUICultures = supportedCultures;
              });

            services.AddSignalR(); // config SignalR
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
            // các middlerware sẽ chạy từ trên xuống dưới
            app.UseHttpsRedirection();

            app.UseImageResizer();  // extension using ImageResizerMiddleware để crop ảnh tối ưu hóa load trang  : https://www.paddo.org/asp-net-core-image-resizing-middleware/
            app.UseStaticFiles(); // hạn chế các file nằm trong root đều không chạy qua middleware tiếp theo
            //app.UseMinResponse(); // sử dụng middleware WebMarkupMin để nén file

            app.UseCookiePolicy(); // các quyền sử dụng cookies cho người dùng bảo vệ thông tin người dùng

            app.UseAuthentication();
            app.UseSession(); //nuget Microsoft.AspNetCore.Session

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>(); // nuget : Microsoft.AspNetCore.Mvc.Localization
            app.UseRequestLocalization(options.Value); //  // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2

            app.UseCors("CorsPolicy"); // config SignalR
            app.UseSignalR(routes =>
            {// config SignalR nuget : Microsoft.AspNetCore.SignalR
                routes.MapHub<Shop3Hub>("/Shop3Hub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // register area
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
            });

            app.UseMiddleware(typeof(VisitorCounterMiddleware));
        }
    }
}