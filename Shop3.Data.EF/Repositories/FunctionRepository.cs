using Shop3.Data.Entities;
using Shop3.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Data.EF.Repositories
{
    // triển khai repository nếu dùng
    public class FunctionRepository : EFRepository<Function, string>, IFunctionRepository
    {
        public FunctionRepository(AppDbContext context) : base(context)
        {

        }
    }
}
