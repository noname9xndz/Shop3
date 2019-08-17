using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
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
using Shop3.Infrastructure.Interfaces;

namespace Shop3.DI
{
    public class DependencyRegister
    {
        public static IServiceProvider RegisterDependencies(IServiceCollection services)
        {
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

            //services.AddRecaptcha(new RecaptchaOptions()
            //{
            //    SiteKey = Configuration["Recaptcha:SiteKey"],
            //    SecretKey = Configuration["Recaptcha:SecretKey"]
            //});

            //nuget : automapper ,AutoMapper.Extensions.Microsoft.DependencyInjection
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });

            services.AddSession(options =>
            {
                //options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true; //1 cookies có được truy cập bởi client script ko
            });

            services.AddImageResizer(); // extension using ImageResizerMiddleware để crop ảnh tối ưu hóa load trang  : https://www.paddo.org/asp-net-core-image-resizing-middleware/
            services.AddAutoMapper();  // nuget : AutoMapper.Extensions.Microsoft.DependencyInjection

            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>(); //khai báo khởi tạo thông tin user, và role
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();

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
