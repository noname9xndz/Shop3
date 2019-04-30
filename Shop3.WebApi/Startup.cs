using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Shop3.Application.AutoMapper;
using Shop3.Application.Implementation;
using Shop3.Application.Interfaces;
using Shop3.Data.EF;
using Shop3.Infrastructure.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace Shop3.WebApi
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

            services.AddDbContext<AppDbContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                  o => o.MigrationsAssembly("Shop3.Data.EF"))); // add thêm options ko dùng project hiện tại mà dùng  Shop3.Data.EF

            services.AddCors(o => o.AddPolicy("Shop3CorsPolicy", builder =>
            {  // https://codeaholicguy.com/2018/05/07/cors-la-gi/
               // http://paginaswebpublicidad.com/questions/3699/lam-the-nao-de-kich-hoat-cors-trong-asp-net-core
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            //nuget : automapper 8.0.0,AutoMapper.Extensions.Microsoft.DependencyInjection 6.0.0
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });
            services.AddAutoMapper();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));



            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));

            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<IProductService, ProductService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
             // config lại object trả về khi respone trả về cho ajax
             .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //nuget : Swashbuckle.AspNetCore
            services.AddSwaggerGen(s =>
            { //https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-2.2
                s.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "NonameShop Project",
                    Description = "Shop3 API Swagger surface",
                    Contact = new Contact { Name = "Noname9xnd", Email = "noname9xnd@gmail.com", Url = "https://github.com/noname9xndz" },
                    License = new License { Name = "Noname9xnd", Url = "https://github.com/noname9xndz" }
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCors("Shop3CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {//https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-2.2
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Project API v1.1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
