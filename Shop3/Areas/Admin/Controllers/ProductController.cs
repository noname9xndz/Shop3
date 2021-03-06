﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Products;
using Shop3.Services;
using Shop3.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace Shop3.Areas.Admin.Controllers
{
    //https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.ihostingenvironment?view=aspnetcore-2.2
    //[Area("Admin")]
    public class ProductController : BaseController
    {
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;
        private readonly IHostingEnvironment _hostingEnvironment; // lấy ra môi trường mà appweb đang chạy
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;
        //todo : tối ưu lại việc xử lý file và excel bằng inject
        public ProductController(IProductService productService, IProductCategoryService productCategoryService
                 ,IHostingEnvironment hostingEnvironment, IFileService fileService, IExcelService excelService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _hostingEnvironment = hostingEnvironment;
            _excelService = excelService;
            _fileService = fileService;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Ajax api

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var model = _productService.GetAllPaging(categoryId, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _productService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductViewModel productVm)
        { 
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
                if (productVm.Id == 0)
                {
                    _productService.Add(productVm);
                }
                else
                {
                    _productService.Update(productVm);
                }
                _productService.Save();
                return new OkObjectResult(productVm);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _productService.Delete(id);
                _productService.Save();

                return new OkObjectResult(id);
            }
        }


        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files, int categoryId)
        {
            // add nuget : epplus.core
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";

                //                if (!Directory.Exists(folder))
                //                {
                //                    Directory.CreateDirectory(folder);
                //                }

                if (!_fileService.CheckDirectoryExist(folder))
                {
                    _fileService.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, filename); // lấy đường dẫn tuyệt đối file

                //using (FileStream fs = System.IO.File.Create(filePath))
                using (FileStream fs = _fileService.CreateFile(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                _productService.ImportExcel(filePath, categoryId);
                _productService.Save();
                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }

        [HttpPost]
        public IActionResult ExportExcel()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"Product_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            //            if (file.Exists)
            //            {
            //                file.Delete();
            //                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            //            }
            if (_fileService.CheckFileExist(file))
            {
                _fileService.DeleteFile(file);
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var products = _productService.GetAll(); //  có thể get bằng search

            //c1 :lấy trực tiếp bằng cách lấy dữ liệu trên form
            //using (ExcelPackage package = new ExcelPackage(file))
            //{
            //    // add a new worksheet to the empty workbook
            //    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");
            //    worksheet.Cells["A1"].LoadFromCollection(products, true, TableStyles.Light1);
            //    worksheet.Cells.AutoFitColumns();
            //    package.Save(); //Save the workbook.
            //}
            _excelService.WriteExcel(file, products);

            // c2 : vẽ trực tiếp trên các cell m m
            //using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath), new FileInfo("đường dẫn file template")))
            //{
            //    //Create the worksheet
            //    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));
            //    //vẽ các cell
            //    ws.Cells["A1"].Value = "tên";
            //    ws.Cells["A2"].Value = "tên";
            //    ws.Cells.AutoFitColumns();
            //    pck.Save();
            //}

            return new OkObjectResult(fileUrl);
        }


        [HttpPost]
        public IActionResult SaveQuantities(int productId, List<ProductQuantityViewModel> quantities)
        {
            _productService.AddQuantity(productId, quantities);
            _productService.Save();
            return new OkObjectResult(quantities);
        }

        [HttpGet]
        public IActionResult GetQuantities(int productId)
        {
            var quantities = _productService.GetQuantities(productId);
            return new OkObjectResult(quantities);
        }

        [HttpPost]
        public IActionResult SaveImages(int productId, string[] imagesAdd,int[] imagesRemove )
        { 
           _productService.AddAndRemoveProductImages(productId,imagesAdd,imagesRemove);
           _productService.Save();
            return new OkObjectResult(imagesAdd);
        }

        [HttpGet]
        public IActionResult GetImages(int productId)
        {
            var images = _productService.GetImages(productId);
            return new OkObjectResult(images);
        }
        [HttpPost]
        public IActionResult DeleteImageProduct(int productImageId)
        {
            _productService.DeleteProductImage(productImageId);
            return new OkResult();
        }

        [HttpPost]
        public IActionResult SaveWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            _productService.AddWholePrice(productId, wholePrices);
            _productService.Save();
            return new OkObjectResult(wholePrices);
        }

        [HttpGet]
        public IActionResult GetWholePrices(int productId)
        {
            var wholePrices = _productService.GetWholePrices(productId);
            return new OkObjectResult(wholePrices);
        }
        #endregion
    }
}