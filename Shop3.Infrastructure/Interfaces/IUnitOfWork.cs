using System;

namespace Shop3.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //dùng savechange của dbcontext
        void Commit();
    }
}
