using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Interfaces;

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


        #endregion
    }
}