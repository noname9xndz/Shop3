using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shop3.Application.AutoMapper;
using Shop3.Application.Implementation;
using Shop3.Application.Interfaces;
using Shop3.Authorization;
using Shop3.Data.EF;
using Shop3.Data.Entities;
using Shop3.Extensions;
using Shop3.Infrastructure.Interfaces;
using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Shop3.Helpers;

namespace Shop3.DI
{
    public class DependencyRegister
    {
        public static IServiceProvider RegisterDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    o => o.MigrationsAssembly("Shop3.Data.EF"))); // add thêm options ko dùng project hiện tại mà dùng  Shop3.Data.EF


            // services.AddMvc();
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.AddMemoryCache();

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

            services.AddRecaptcha(new RecaptchaOptions()
            {
                SiteKey = configuration["Recaptcha:SiteKey"],
                SecretKey = configuration["Recaptcha:SecretKey"]
            });

            //nuget : automapper ,AutoMapper.Extensions.Microsoft.DependencyInjection
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainToViewModelMappingProfile());
                mc.AddProfile(new ViewModelToDomainMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSession(options =>
            {
                //options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true; //1 cookies có được truy cập bởi client script ko
            });

            services.AddImageResizer(); // extension using ImageResizerMiddleware để crop ảnh tối ưu hóa load trang  : https://www.paddo.org/asp-net-core-image-resizing-middleware/
            services.AddAutoMapper();  // nuget : AutoMapper.Extensions.Microsoft.DependencyInjection

            services.AddImageResizer(); // extension using ImageResizerMiddleware để crop ảnh tối ưu hóa load trang  : https://www.paddo.org/asp-net-core-image-resizing-middleware/
            services.AddAutoMapper();  // nuget : AutoMapper.Extensions.Microsoft.DependencyInjection

            services.AddAuthentication() // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/?view=aspnetcore-2.2&tabs=visual-studio
                .AddFacebook(facebookOpts =>
                {
                    // config login bằng fb ,gg
                    // tham khảo  : https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/other-logins?view=aspnetcore-2.2
                    facebookOpts.AppId = configuration["Authentication:Facebook:AppId"];
                    facebookOpts.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(googleOpts =>
                {
                    googleOpts.ClientId = configuration["Authentication:Google:ClientId"];
                    googleOpts.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                });

            //services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>(); //khai báo khởi tạo thông tin user, và role
            //services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();

            var containerBuilder = new ContainerBuilder();

            DependencyConfigure(containerBuilder);

            //populate Autofac container builder with the set of registered service descriptors
            containerBuilder.Populate(services);

            //create service provider
            IServiceProvider serviceProvider = new AutofacServiceProvider(containerBuilder.Build());
            return serviceProvider;
        }

        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="container"></param>
        protected static void DependencyConfigure(ContainerBuilder container)
        {
            container.RegisterAssemblyModules();
            //container.RegisterType<JmDbContext>().As<IDbContext>().InstancePerLifetimeScope();
            //container.Register<IDbContext>(c => new JmDbContext()).InstancePerLifetimeScope();
            // dynamic repository configured
            container.RegisterGeneric(typeof(EFUnitOfWork)).As(typeof(IUnitOfWork)).InstancePerLifetimeScope();
            container.RegisterGeneric(typeof(EFRepository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();

            // todo: configure custom dependencies here
            container.RegisterType<ProductCategoryService>().As<IProductCategoryService>().InstancePerLifetimeScope();
            container.RegisterType<BaseResourceAuthorizationHandler>().As<IAuthorizationHandler>().InstancePerLifetimeScope();

        }
    }
}
