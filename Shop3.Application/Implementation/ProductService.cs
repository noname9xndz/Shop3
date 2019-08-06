using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Common;
using Shop3.Application.ViewModels.Custom;
using Shop3.Application.ViewModels.Products;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.Infrastructure.Interfaces;
using Shop3.Utilities.Constants;
using Shop3.Utilities.Dtos;
using Shop3.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shop3.Application.Implementation
{
    public class ProductService : IProductService
    {
        private IRepository<Product, int> _productRepository;
        private IRepository<Tag, string> _tagRepository;
        private IRepository<ProductTag, int> _productTagRepository;
        private IRepository<ProductQuantity, int> _productQuantityRepository;
        IRepository<ProductImage, int> _productImageRepository;
        private IRepository<WholePrice, int> _wholePriceRepository;
        private IRepository<Bill, int> _billRepository;
        private IRepository<BillDetail, int> _billDetailRepository;
        private IRepository<WishProduct, int> _wishProductRepository;
        private readonly UserManager<AppUser> _userManager;

        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IRepository<Product, int> productRepository,
            IRepository<Tag, string> tagRepository,
            IUnitOfWork unitOfWork,
            IRepository<ProductTag, int> productTagRepository,
            IRepository<ProductQuantity, int> productQuantityRepository,
            IRepository<ProductImage, int> productImageRepository,
            IRepository<WholePrice, int> wholePriceRepository,
            IRepository<Bill, int> billRepository,
            IRepository<BillDetail, int> billDetailRepository,
            IRepository<WishProduct, int> wishProductRepository,
            UserManager<AppUser> userManager,
        IMapper mapper
            )
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _productTagRepository = productTagRepository;
            _unitOfWork = unitOfWork;
            _productQuantityRepository = productQuantityRepository;
            _productImageRepository = productImageRepository;
            _wholePriceRepository = wholePriceRepository;
            _billRepository = billRepository;
            _billDetailRepository = billDetailRepository;
            _wishProductRepository = wishProductRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public PagedResult<ProductViewModel> GetAllPagingWithSortKey(int? categoryId, string sortkey, int page, int pageSize)
        {
            var query = _productRepository.FindAll(x => x.Status == Status.Active || x.Status == Status.InActive);

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            int totalRow = query.Count();

            #region Sort Key

            if (sortkey == CommonConstants.SortKey.Lastest || sortkey == null)
            {
                query = query.OrderByDescending(x => x.DateCreated);
            }
            if (sortkey == CommonConstants.SortKey.NameZA)
            {
                query = query.OrderByDescending(x => x.Name);
            }
            if (sortkey == CommonConstants.SortKey.NameAZ)
            {
                query = query.OrderBy(x => x.Name);
            }
            if (sortkey == CommonConstants.SortKey.Pricehightolow)
            {
                query = query.OrderByDescending(x => x.Price);
            }
            if (sortkey == CommonConstants.SortKey.Pricelowtohigh)
            {
                query = query.OrderBy(x => x.Price);
            }


            #endregion


            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            //var data = query.ProjectTo<ProductViewModel>().ToList();
            var data = _mapper.ProjectTo<ProductViewModel>(query).ToList();

            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public ProductViewModel Add(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }

                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
                var product = _mapper.Map<ProductViewModel, Product>(productVm);
                foreach (var productTag in productTags)
                {
                    product.ProductTags.Add(productTag);
                }
                _productRepository.Add(product);

            }
            return productVm;
        }

        public void Delete(int id)
        {
            _productRepository.RemoveById(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            //return _productRepository.FindAll(x => x.ProductCategory).ProjectTo<ProductViewModel>().ToList();
            return _mapper.ProjectTo<ProductViewModel>(_productRepository.FindAll(x => x.ProductCategory)).ToList();
        }

        public PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var query = _productRepository.FindAll(x => x.Status == Status.Active || x.Status == Status.InActive);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));
            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.DateCreated)
                        .Skip((page - 1) * pageSize).Take(pageSize);

            //var data = query.ProjectTo<ProductViewModel>().ToList();
            var data = _mapper.ProjectTo<ProductViewModel>(query).ToList();

            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public ProductViewModel GetById(int id)
        {
            return _mapper.Map<Product, ProductViewModel>(_productRepository.FindById(id));
        }


        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();

            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }
                    _productTagRepository.RemoveMultiple(_productTagRepository.FindAll(x => x.Id == productVm.Id).ToList());
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
            }

            var product = _mapper.Map<ProductViewModel, Product>(productVm);
            foreach (var productTag in productTags)
            {
                product.ProductTags.Add(productTag);
            }
            _productRepository.Update(product.Id, product);
        }

        // add nuget : epplus.core
        public void ImportExcel(string filePath, int categoryId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];// mảng trong excel bắt đầu từ 1
                Product product;
                // dòng 1 trong execl là header => chạy từ dòng 2
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    // tạo mới 1 product và đọc từng row => mapping
                    product = new Product();
                    product.CategoryId = categoryId;

                    product.Name = workSheet.Cells[i, 1].Value.ToString();//dòng i cột 1

                    product.Description = workSheet.Cells[i, 2].Value.ToString();
                    //TryParse tránh trường hợp throw exception thay vào đó là đưa về giá trị mặc đinh là 0
                    decimal.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var originalPrice); // c# 7 có thể out luôn không cần khai báo
                    product.OriginalPrice = originalPrice;

                    decimal.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var price);
                    product.Price = price;
                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var promotionPrice);

                    product.PromotionPrice = promotionPrice;
                    product.Content = workSheet.Cells[i, 6].Value.ToString();
                    product.SeoKeywords = workSheet.Cells[i, 7].Value.ToString();

                    product.SeoDescription = workSheet.Cells[i, 8].Value.ToString();
                    bool.TryParse(workSheet.Cells[i, 9].Value.ToString(), out var hotFlag);

                    product.HotFlag = hotFlag;
                    bool.TryParse(workSheet.Cells[i, 10].Value.ToString(), out var homeFlag);
                    product.HomeFlag = homeFlag;

                    product.Status = Status.Active;

                    _productRepository.Add(product);
                }
            }
        }

        public List<ProductQuantityViewModel> GetQuantities(int productId)
        {
            return _mapper.ProjectTo<ProductQuantityViewModel>(_productQuantityRepository.FindAll(x => x.ProductId == productId)).ToList();
        }

        public void AddQuantity(int productId, List<ProductQuantityViewModel> quantities)
        {
            // xóa số lượng sp có sẵn
            _productQuantityRepository.RemoveMultiple(_productQuantityRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach (var quantity in quantities)
            {
                _productQuantityRepository.Add(new ProductQuantity()
                {
                    ProductId = productId,
                    ColorId = quantity.ColorId,
                    SizeId = quantity.SizeId,
                    Quantity = quantity.Quantity
                });
            }
        }

        public List<ProductImageViewModel> GetImages(int productId)
        {
            return _mapper.ProjectTo<ProductImageViewModel>(_productImageRepository.FindAll(x => x.ProductId == productId)).ToList();
        }

        public void AddImages(int productId, string[] images)
        {
            if (images.Length > 0)
            {
                //_productImageRepository.RemoveMultiple(_productImageRepository.FindAll(x => x.ProductId == productId).ToList());
                foreach (var image in images)
                {
                    _productImageRepository.Add(new ProductImage()
                    {
                        Path = image,
                        ProductId = productId,
                        Caption = string.Empty
                    });
                }
            }
            //_productImageRepository.RemoveMultiple(_productImageRepository.FindAll(x => x.ProductId == productId).ToList());
            //foreach (var image in images)
            //{
            //    _productImageRepository.Add(new ProductImage()
            //    {
            //        Path = image,
            //        ProductId = productId,
            //        Caption = string.Empty
            //    });
            //}
        }

        public void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            _wholePriceRepository.RemoveMultiple(_wholePriceRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach (var wholePrice in wholePrices)
            {
                _wholePriceRepository.Add(new WholePrice()
                {
                    ProductId = productId,
                    FromQuantity = wholePrice.FromQuantity,
                    ToQuantity = wholePrice.ToQuantity,
                    Price = wholePrice.Price
                });
            }
        }

        public List<WholePriceViewModel> GetWholePrices(int productId)
        {
            return _mapper.ProjectTo<WholePriceViewModel>(_wholePriceRepository.FindAll(x => x.ProductId == productId)).ToList();
        }

        public List<ProductViewModel> GetLastest(int top)
        {
            //var query = from x in _productRepository.FindAll()
            //            join y in _billDetailRepository.FindAll().OrderByDescending(y => y.Quantity) on x.Id equals y.ProductId into xy
            //            from a2 in xy.DefaultIfEmpty()
            //            join z in _billRepository.FindAll(z => z.Status == Status.Active) on a2.BillId equals z.Id into xyz
            //            from a3 in xyz.DefaultIfEmpty()
            //            select x;

            var query = from x in _productRepository.FindAll()
                        join y in _billDetailRepository.FindAll() on x.Id equals y.ProductId
                        join z in _billRepository.FindAll(z => z.Status == Status.Active) on y.BillId equals z.Id
                        orderby y.Quantity descending
                        select x;

            return _mapper.ProjectTo<ProductViewModel>(query.Take(top)).ToList();




        }

        public List<ProductViewModel> GetHotProduct(int top)
        {
            var query = _productRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(top);

            return _mapper.ProjectTo<ProductViewModel>(query).ToList();
        }

        public List<ProductViewModel> GetRelatedProducts(int id, int top)
        {
            var product = _productRepository.FindById(id);
            var query = _productRepository.FindAll(x => x.Status == Status.Active
                                                        && x.Id != id && x.CategoryId == product.CategoryId)
                .OrderByDescending(x => x.DateCreated)
                .Take(top);

            return _mapper.ProjectTo<ProductViewModel>(query).ToList();
        }

        public List<ProductViewModel> GetUpsellProducts(int top)
        {
            var query = _productRepository.FindAll(x => x.PromotionPrice != null)
                .OrderByDescending(x => x.DateModified)
                .Take(top);
            return _mapper.ProjectTo<ProductViewModel>(query).ToList();
        }

        public List<TagViewModel> GetProductTags(int productId)
        {
            var tags = _tagRepository.FindAll();
            var productTags = _productTagRepository.FindAll();

            var query = from t in tags
                        join pt in productTags
                        on t.Id equals pt.TagId
                        where pt.ProductId == productId
                        select new TagViewModel()
                        {
                            Id = t.Id,
                            Name = t.Name
                        };
            return query.ToList();
        }

        public bool CheckAvailability(int productId, int size, int color)
        {
            var quantity = _productQuantityRepository.FindSingle(x => x.ColorId == color && x.SizeId == size && x.ProductId == productId);
            if (quantity == null)
                return false;
            return quantity.Quantity > 0;
        }

        public List<ProductViewModel> GetNewProduct(int top)
        {
            var query = _productRepository.FindAll(x => x.Status == Status.Active).OrderByDescending(x => x.DateCreated)
                 .Take(top);

            return _mapper.ProjectTo<ProductViewModel>(query).ToList();
        }

        public List<ProductViewModel> GetSpecialOfferProduct(int top)
        {
            var query = _productRepository.FindAll(x => x.Status == Status.Active)

                .OrderBy(x => x.PromotionPrice).Where(e => e.PromotionPrice != null)
                .Take(top);
            return _mapper.ProjectTo<ProductViewModel>(query).ToList();
        }

        public PagedResult<ProductViewModel> GetAllWishListPaging(Guid Id, int page, int pageSize)
        {
            var query = from p in _productRepository.FindAll()
                        join wp in _wishProductRepository.FindAll() on p.Id equals wp.ProductId
                        join u in _userManager.Users.Where(x => x.Id == Id) on wp.CustomerId equals u.Id
                        where wp.CustomerId == u.Id && p.Status == Status.Active
                        select p;

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = _mapper.ProjectTo<ProductViewModel>(data).ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public void AddWish(WishProductViewModel product)
        {
            var data = _mapper.Map<WishProductViewModel, WishProduct>(product);
            _wishProductRepository.Add(data);
            _unitOfWork.Commit();
        }

        public void DeleteWishProduct(int productId, Guid Id)
        {
            if (CheckWishProduct(productId, Id) == true)
            {
                var res = (from p in _wishProductRepository.FindAll()
                           where p.ProductId == productId && p.CustomerId == Id
                           select p.Id).Take(1);
                int wishId = Convert.ToInt32(res.FirstOrDefault());
                _wishProductRepository.RemoveById(wishId);
                _unitOfWork.Commit();
            }
        }

        public WishProductViewModel GetWishProductById(int id, Guid Id)
        {
            return _mapper.Map<WishProduct, WishProductViewModel>(_wishProductRepository.FindById(id));
        }

        public bool CheckWishProduct(int productId, Guid Id)
        {
            if (_wishProductRepository.FindAll(x => x.ProductId == productId && x.CustomerId == Id).ToList().Count > 0)
            {
                return true;
            }

            return false;
        }

        public List<ProductViewModel> GetAllWishProduct(Guid Id)
        {
            var data = from p in _productRepository.FindAll()
                       join wp in _wishProductRepository.FindAll(x => x.CustomerId == Id) on p.Id equals wp.ProductId
                       select p;

            return _mapper.ProjectTo<ProductViewModel>(data).ToList();

        }

        public List<CustomProductTagViewModel> GetProductWithTagRanDom(int top)
        {
            var data = (from t in _tagRepository.FindAll()
                        join pt in _productTagRepository.FindAll() on t.Id equals pt.TagId
                        join p in _productRepository.FindAll() on pt.ProductId equals p.Id
                        orderby Guid.NewGuid()
                        select new CustomProductTagViewModel()
                        {
                            ProductId = p.Id,
                            SeoAlias = p.SeoAlias,
                            TagName = t.Name
                        }).Take(top);

            return _mapper.ProjectTo<CustomProductTagViewModel>(data).ToList();
        }

        public void DeleteProductImage(int productImg)
        {
           _productImageRepository.RemoveById(productImg);
           _unitOfWork.Commit();
        }


        public List<CustomSellProductViewModel> GetProductsByStatusBill(int top, BillStatus status)
        {


                var product = _productRepository.FindAll();
                var billdetails = _billDetailRepository.FindAll();
                var bill = _billRepository.FindAll(x=>x.BillStatus == status);

                //var a = _billDetailRepository.FindAll().Include(x=>x.Bill).Include(x => x.Product);

                var query = (from p in product
                             join bd in billdetails on p.Id equals bd.ProductId
                             join b in bill on bd.BillId equals b.Id
                             group p by new { p.Id, p.Name, p.Unit, p.Image, p.PromotionPrice, p.Price } into sellProduct
                             select new CustomSellProductViewModel
                             {
                                 Id = sellProduct.Key.Id,
                                 Image = sellProduct.Key.Image,
                                 Price = (sellProduct.Key.PromotionPrice > 0 && sellProduct.Key.PromotionPrice < sellProduct.Key.Price) ? sellProduct.Key.PromotionPrice.Value : sellProduct.Key.Price,
                                 Name = sellProduct.Key.Name,
                                 Unit = sellProduct.Key.Unit,
                                 Total = (sellProduct.Count(pt => pt.Id != null))
                             });
                var customSellProduct = query.OrderByDescending(x => x.Total).Take(top).ToList();
                return customSellProduct; ;

          

            
        }

        public void AddAndRemoveProductImages(int productId, string[] imagesAdd, int[] productImageIdRemove)
        {
            if (imagesAdd.Length > 0)
            {
                foreach (var image in imagesAdd)
                {
                    _productImageRepository.Add(new ProductImage()
                    {
                        Path = image,
                        ProductId = productId,
                        Caption = string.Empty
                    });
                }
            }
            if (productImageIdRemove.Length > 0)
            {
                List<int> productImageId = productImageIdRemove.OfType<int>().ToList();
                _productImageRepository.RemoveMultipleWithListKey(productImageId);
               
            }
        }
    }
}
