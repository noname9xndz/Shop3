using Shop3.Application.Dapper.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop3.Application.Dapper.SqlCommands
{
    public interface IReportService
    {
        Task<IEnumerable<RevenueReportViewModel>> GetReportAsync(string fromDate, string toDate);
    }
}
