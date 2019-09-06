using Shop3.DependencyResolver;
using Shop3.Application.Dapper.Implementation;
using System.ComponentModel.Composition;
using Shop3.Application.Dapper.SqlCommands;

namespace Shop3.Application.Dapper
{

    [Export(typeof(IDependencyResolver))]
    public class ApplicationDependencyResolver : IDependencyResolver
    {
        public void SetUp(IDependencyRegister dependencyRegister)
        {
             dependencyRegister.AddTransient<IReportService, ReportService>();
        }
    }
}
