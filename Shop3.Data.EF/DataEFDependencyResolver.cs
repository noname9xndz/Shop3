using Shop3.DependencyResolver;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Shop3.Data.EF
{
    [Export(typeof(IDependencyResolver))]
    class DataDependencyResolver : IDependencyResolver
    {
            public void SetUp(IDependencyRegister dependencyRegister)
            {
            //dependencyRegister.AddScoped<IDummyService1, DummyService1>();

            //services.AddTransient<DbInitializer>();
            //services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            //services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));
        }
        
    }
}
