using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //dùng savechange của dbcontext
        void Commit();
    }
}
