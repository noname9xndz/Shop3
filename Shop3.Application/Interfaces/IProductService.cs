using Shop3.Application.ViewModels.Common;
using Shop3.Application.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();
        
    }
}
