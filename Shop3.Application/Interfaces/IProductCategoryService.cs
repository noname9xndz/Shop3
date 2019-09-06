using Shop3.Application.ViewModels.Products;
using System.Collections.Generic;

namespace Shop3.Application.Interfaces
{
    // trả trực tiếp về viewmodel sau đó mapping trực tiếp vào model => controller chỉ việc gọi dùng
    public interface IProductCategoryService
    {
        ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm);

        void Update(ProductCategoryViewModel productCategoryVm);

        void Delete(int id);

        List<ProductCategoryViewModel> GetAll();

        List<ProductCategoryViewModel> GetAll(string keyword);

        List<ProductCategoryViewModel> GetAllByParentId(int parentId);

        ProductCategoryViewModel GetById(int id);

        // xử lý khi phần tử được thả vào node khác : id nguồn , id đích , list các item sẽ update
        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);

        // xử lý khi các node được đổi chỗ => sắp xếp thứ tự  của các category : id nguồn ,id đích
        void ReOrder(int sourceId, int targetId);

        List<ProductCategoryViewModel> GetHomeCategories(int top);

        bool CheckParent(int id);



        void Save();
    }
}
