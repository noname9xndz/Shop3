﻿using AutoMapper;
using Shop3.Application.ViewModels.Blogs;
using Shop3.Application.ViewModels.Common;
using Shop3.Application.ViewModels.Products;
using Shop3.Application.ViewModels.System;
using Shop3.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        // setup mapping trực tiếp giữa  các entityViewModel và entityModel
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<Announcement, AnnouncementViewModel>().MaxDepth(2);

            CreateMap<Function, FunctionViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();
            CreateMap<Bill, BillViewModel>();
            CreateMap<BillDetail, BillDetailViewModel>();
            CreateMap<Color, ColorViewModel>();
            CreateMap<Size, SizeViewModel>();

            //
            /* ví dụ select bill nhưng lại include BillDetail vào csdl và 
             * ngược lại : tránh trường hợp chạy vòng vô tận giữa 2 thằng này 
             * bản chất là giới hạn việc mapping giữa 2 thằng này tránh trường hợp chạy vòng throw ra lỗi
            CreateMap<Bill, BillViewModel>().MaxDepth(2);
            CreateMap<BillDetail, BillDetailViewModel>().MaxDepth(2);
            CreateMap<Color, ColorViewModel>().MaxDepth(2);
            CreateMap<Size, SizeViewModel>().MaxDepth(2);
             */
            //MaxDepth(2) tránh việc lặp vô tận chỉ cho đi sâu vào 2 tầng
            CreateMap<ProductQuantity, ProductQuantityViewModel>().MaxDepth(2);
            CreateMap<ProductImage, ProductImageViewModel>().MaxDepth(2);
            CreateMap<WholePrice, WholePriceViewModel>().MaxDepth(2);

            CreateMap<Blog, BlogViewModel>().MaxDepth(2);
            CreateMap<BlogTag, BlogTagViewModel>().MaxDepth(2);
            CreateMap<Slide, SlideViewModel>().MaxDepth(2);
            CreateMap<SystemConfig, SystemConfigViewModel>().MaxDepth(2);
            CreateMap<Footer, FooterViewModel>().MaxDepth(2);

            CreateMap<Feedback, FeedbackViewModel>().MaxDepth(2);
            CreateMap<Contact, ContactViewModel>().MaxDepth(2);
            CreateMap<Page, PageViewModel>().MaxDepth(2);

        }
    }
}
