using Shop3.Application.ViewModels.Common;
using Shop3.Application.ViewModels.Products;
using Shp3.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);

        ProductViewModel Add(ProductViewModel product);

        void Update(ProductViewModel product);

        void Delete(int id);

        ProductViewModel GetById(int id);

        void Save();
    }
}
