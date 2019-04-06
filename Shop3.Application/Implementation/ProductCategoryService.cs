using AutoMapper;
using AutoMapper.QueryableExtensions;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Products;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop3.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        // không triển khai riêng IProductCategoryRepository
        // IRepository được viết dưới dạng generic chỉ cần chỉ ra : thực thể và key của nó
        //  mapping trực tiếp từ viewmodel vào model hoặc model qua view model => controller chỉ việc gọi dùng
        // 1 service sinh ra phải register trong startup
        private IRepository<ProductCategory, int> _productCategoryRepository;
        private IUnitOfWork _unitOfWork;

        public ProductCategoryService(IRepository<ProductCategory, int> productCategoryRepository, IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _productCategoryRepository.Add(productCategory);
            return productCategoryVm;
        }

        public bool CheckParent(int id)
        {
            //_productCategoryRepository.FindById(id)
            //var query = _productCategoryRepository.FindAll(x => x.Id == id).Where(x => x.ParentId.HasValue);
            if (_productCategoryRepository.FindAll(x => x.Id == id).Where(x => x.ParentId.HasValue).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
              

        }

        public void Delete(int id)
        {
           
             _productCategoryRepository.Remove(id);
            
            
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            return _productCategoryRepository.FindAll().OrderBy(x => x.ParentId)
                 .ProjectTo<ProductCategoryViewModel>().ToList(); // mapping từ model qua viewmodel
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _productCategoryRepository.FindAll(x => x.Name.Contains(keyword)
                         || x.Description.Contains(keyword)).OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>().ToList(); 
            else
                return _productCategoryRepository.FindAll().OrderBy(x => x.ParentId)
                             .ProjectTo<ProductCategoryViewModel>().ToList(); // mapping từ model qua viewmodel
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _productCategoryRepository.FindAll(x => x.Status == Status.Active && x.ParentId == parentId)
                                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return Mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryRepository.FindById(id));
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _productCategoryRepository
                .FindAll(x => x.HomeFlag == true, c => c.Products)
                .OrderBy(x => x.HomeOrder)
                .Take(top).ProjectTo<ProductCategoryViewModel>();

            var categories = query.ToList();
            foreach (var category in categories)
            {
                //category.Products = _productRepository
                //    .FindAll(x => x.HotFlag == true && x.CategoryId == category.Id)
                //    .OrderByDescending(x => x.DateCreated)
                //    .Take(5)
                //    .ProjectTo<ProductViewModel>().ToList();
            }
            return categories;
        }

        // xử lý khi các node được đổi chỗ => sắp xếp thứ tự  của các category : id nguồn ,id đích
        public void ReOrder(int sourceId, int targetId)
        {
            var source = _productCategoryRepository.FindById(sourceId);
            var target = _productCategoryRepository.FindById(targetId);
            // đổi chỗ 2 phần tử 
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _productCategoryRepository.Update(source);
            _productCategoryRepository.Update(target);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _productCategoryRepository.Update(productCategory);
        }

        // xử lý khi phần tử được thả vào node khác : id nguồn , id đích , list các item sẽ update
        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _productCategoryRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;
            _productCategoryRepository.Update(sourceCategory);

            //Lấy ra các phần tử ngang hàng(anh em) với những thằng được thả
            var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id]; // lấy từ Dictionary với key là id truyền vào
                _productCategoryRepository.Update(child);
            }
        }
    }
}
