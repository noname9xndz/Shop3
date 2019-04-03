using AutoMapper;
using AutoMapper.QueryableExtensions;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Common;
using Shop3.Application.ViewModels.Products;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.Infrastructure.Interfaces;
using Shp3.Utilities.Constants;
using Shp3.Utilities.Dtos;
using Shp3.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shop3.Application.Implementation
{
    public class ProductService : IProductService
    {
        private IRepository<Product, int> _productRepository;
     

        public ProductService(IRepository<Product, int> productRepository)
        {
            _productRepository = productRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            return _productRepository.FindAll(x => x.ProductCategory).ProjectTo<ProductViewModel>().ToList();
        }

        public PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var query = _productRepository.FindAll(x => x.Status == Status.Active);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));
            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.DateCreated)
                        .Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.ProjectTo<ProductViewModel>().ToList();

            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

    }
}
