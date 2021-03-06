﻿using Shop3.Application.ViewModels.Common;
using Shop3.Application.ViewModels.Products;
using Shop3.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shop3.Application.ViewModels.Custom;
using Shop3.Data.Enums;

namespace Shop3.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);
        PagedResult<ProductViewModel> GetAllPagingWithSortKey(int? categoryId, string sortkey, int page, int pageSize);

        ProductViewModel Add(ProductViewModel product);

        void Update(ProductViewModel product);

        void Delete(int id);

        ProductViewModel GetById(int id);

        void ImportExcel(string filePath, int categoryId);

        void AddQuantity(int productId, List<ProductQuantityViewModel> quantities);

        List<ProductQuantityViewModel> GetQuantities(int productId);

        void AddImages(int productId, string[] images);

        void AddAndRemoveProductImages(int productId, string[] imagesAdd, int[] productImageIdRemove);

        List<ProductImageViewModel> GetImages(int productId);

        void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices);

        List<WholePriceViewModel> GetWholePrices(int productId);

        List<ProductViewModel> GetLastest(int top);

        List<ProductViewModel> GetHotProduct(int top);

        List<ProductViewModel> GetSpecialOfferProduct(int top);

        List<ProductViewModel> GetNewProduct(int top);

        List<ProductViewModel> GetRelatedProducts(int id, int top);

        List<ProductViewModel> GetUpsellProducts(int top);
        //List<CustomSellProductViewModel> GetTopsellProducts(int top);
        List<CustomSellProductViewModel> GetProductsByStatusBill(int top,BillStatus status);


        List<TagViewModel> GetProductTags(int productId);

        bool CheckAvailability(int productId, int size, int color);

        void Save();

        PagedResult<ProductViewModel> GetAllWishListPaging(Guid id, int page, int pageSize);

        void AddWish(WishProductViewModel product);

        void DeleteWishProduct(int wishProductId, Guid id);

        WishProductViewModel GetWishProductById(int id, Guid Id);

        bool CheckWishProduct(int productId,Guid id);

        List<ProductViewModel> GetAllWishProduct(Guid id);

        List<CustomProductTagViewModel> GetProductWithTagRanDom(int top);

       void DeleteProductImage(int productImg);


    }
}
