using Shop3.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop3.Application.Interfaces
{
    

    public interface IFunctionService : IDisposable
    {
        Task<List<FunctionViewModel>> GetAll();

    }
}
