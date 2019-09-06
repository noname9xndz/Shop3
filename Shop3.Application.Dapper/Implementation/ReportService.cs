using Dapper;
using Microsoft.Extensions.Configuration;
using Shop3.Application.Dapper.SqlCommands;
using Shop3.Application.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Shop3.Application.Dapper.Implementation
{
    // http://www.c-sharp.vn/dot-net/dapper-getting-started-061d3d
    // http://hanhtranglaptrinh.vn6.vn/dapper-c-la-gi-micro-orm-trong-net/
    // Dapper là một ORM (Object Relational Mapping) Framework cho .Net, giống như Entity Frameworkđược xây dựng để liên kết các bảng trên database với các đối tượng trong project.
    // Input là Sql Query hoặc Stored Procedure, output là Entities, Có 2 cách tiếp cận ORM: Code First , Database First 
    // nugget : Dapper , Microsoft.Extensions.Configuration
    public class ReportService : IReportService
    {
        private readonly IConfiguration _configuration;

        public ReportService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IEnumerable<RevenueReportViewModel>> GetReportAsync(string fromDate, string toDate)
        {
            // todo bug custom datetime on client input
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await sqlConnection.OpenAsync();  // open conn

                var dynamicParameters = new DynamicParameters();
                var now = DateTime.Now;

                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1); // ngày đầu tiên trong  của tháng hiện tại
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1); // ngày hiện tại -1 

                dynamicParameters.Add("@fromDate", string.IsNullOrEmpty(fromDate) ? firstDayOfMonth.ToString("MM/dd/yyyy") : fromDate);
                dynamicParameters.Add("@toDate", string.IsNullOrEmpty(toDate) ? lastDayOfMonth.ToString("MM/dd/yyyy") : toDate);

                try
                {
                    // use prc
                    return await sqlConnection.QueryAsync<RevenueReportViewModel>("GetRevenueDaily", dynamicParameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
