using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Shop3.Application.AutoMapper;
using Shop3.Application.Implementation;
using Shop3.Application.Interfaces;
using Shop3.Data.EF;
using Shop3.Data.Entities;
using Shop3.Infrastructure.Interfaces;
using Shop3.WebApi.Helpers;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;
using Shop3.DependencyResolver;
using Contact = Swashbuckle.AspNetCore.Swagger.Contact;

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

            //Config authen , sử dụng token để đăng nhập
            services.AddAuthentication(o =>
            { // https://www.ecanarys.com/Blogs/ArticleID/308/Token-Based-Authentication-for-Web-APIs
              // http://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api
              // https://fullstackmark.com/post/19/jwt-authentication-flow-with-refresh-tokens-in-aspnet-core-web-api
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;

                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                };
            });

           
            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new DomainToViewModelMappingProfile());
            //    mc.AddProfile(new ViewModelToDomainMappingProfile());
            //});

            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);

            services.AddAutoMapper();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(
                sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService)
                );

            
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>(); // register cơ chế ghi đè ClaimsPrincipal



            services.LoadDependencies(Configuration["DI:Path"], Configuration["DI:Shop3DataEF:Dll"]);
            services.LoadDependencies(Configuration["DI:Path"], Configuration["DI:Shop3Application:Dll"]);
            services.LoadDependencies(Configuration["DI:Path"], Configuration["DI:Shop3ApplicationDapper:Dll"]);


           // services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
             // config lại object trả về khi respone trả về cho ajax
             .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //nuget : Swashbuckle.AspNetCore 
            services.AddSwaggerGen(s =>
            { //https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-2.2
              // https://localhost:44303/swagger
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
            app.UseAuthentication();

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
