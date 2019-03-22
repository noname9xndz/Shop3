using Shop3.Data.Entities;
using Shop3.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Data.IRepositories
{
    //  khai báo interface repository nếu dùng
    public interface IFunctionRepository : IRepository<Function, string>
    {

    }
}
