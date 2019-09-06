using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Products;
using Shop3.Utilities.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Shop3.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        IProductCategoryService _productCategoryService;
        public ProductCategoryController(IProductService productService, IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpPost]
        // xử lý khi phần tử được thả vào node khác : id nguồn , id đích , list các item sẽ update
        public IActionResult UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _productCategoryService.UpdateParentId(sourceId, targetId, items);
                    _productCategoryService.Save();
                    return new OkResult();
                }
            }
        }

        [HttpPost]
        // xử lý khi các node được đổi chỗ => sắp xếp thứ tự  của các category : id nguồn ,id đích
        public IActionResult ReOrder(int sourceId, int targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _productCategoryService.ReOrder(sourceId, targetId);
                    _productCategoryService.Save();
                    return new OkResult();
                }
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _productCategoryService.GetById(id);

            return new ObjectResult(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                if (_productCategoryService.CheckParent(id) == true)
                {
                    // return new BadRequestResult();
                    return Json(new { status = "error", message = "no delete" });
                }
                else
                {

                    _productCategoryService.Delete(id);
                    _productCategoryService.Save();
                    return new OkObjectResult(id);
                }

            }
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductCategoryViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                // selet lỗi trả về cho view
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name); // chuyển tên => seoalias không dấu
                if (productVm.Id == 0) // có 1 product
                {
                    _productCategoryService.Add(productVm);
                }
                else
                {
                    _productCategoryService.Update(productVm);
                }
                _productCategoryService.Save();
                return new OkObjectResult(productVm);

            }
        }


        #endregion
    }
}