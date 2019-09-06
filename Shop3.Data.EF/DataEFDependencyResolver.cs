using Shop3.DependencyResolver;
using System.ComponentModel.Composition;
using Microsoft.AspNetCore.Identity;
using Shop3.Data.Entities;
using Shop3.Infrastructure.Interfaces;

namespace Shop3.Data.EF
{
    [Export(typeof(IDependencyResolver))]
    class DataDependencyResolver : IDependencyResolver
    {
        public void SetUp(IDependencyRegister dependencyRegister)
        {
            

            dependencyRegister.AddTransient<DbInitializer>();
            dependencyRegister.AddScoped<UserManager<AppUser>, UserManager<AppUser>>(); //khai báo khởi tạo thông tin user, và role
            dependencyRegister.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>(); //AddScoped giới hạn 1 request gửi lên
            dependencyRegister.AddTransientWithType(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            dependencyRegister.AddTransientWithType(typeof(IRepository<,>), typeof(EFRepository<,>));
        }

    }
}
