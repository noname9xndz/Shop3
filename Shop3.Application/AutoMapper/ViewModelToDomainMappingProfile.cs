using AutoMapper;
using Shop3.Application.ViewModels.Blogs;
using Shop3.Application.ViewModels.Common;
using Shop3.Application.ViewModels.Products;
using Shop3.Application.ViewModels.System;
using Shop3.Data.Entities;
using System;

namespace Shop3.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        // sử dụng contructor để mapping bằng ConstructUsing=> viết thêm contructor trong các entity để gán các giá trị truyền vào
        public ViewModelToDomainMappingProfile()
        {
           
            CreateMap<ProductCategoryViewModel, ProductCategory>()
                .ConstructUsing(
                     c => new ProductCategory(
                         c.Name, c.Description, c.ParentId, c.HomeOrder, c.Image, c.HomeFlag,
                        c.SortOrder, c.Status, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription
                  ));

            CreateMap<ProductViewModel, Product>()
             .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Image, c.Price, c.OriginalPrice,
                c.PromotionPrice, c.Description, c.Content, c.HomeFlag, c.HotFlag, c.Tags, c.Unit, c.Status,
                c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

            CreateMap<AppUserViewModel, AppUser>() // c.Id.GetValueOrDefault(Guid.Empty) cho phép id null
             .ConstructUsing(c => new AppUser(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.UserName,
               c.Email, c.PhoneNumber, c.Avatar, c.Status));  //public string Adress { get; set; } todo


            CreateMap<PermissionViewModel, Permission>()
             .ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete));
            
            CreateMap<BillViewModel, Bill>()
              .ConstructUsing(c => new Bill(c.Id, c.CustomerName, c.CustomerAddress,
              c.CustomerMobile, c.CustomerMessage, c.BillStatus,
              c.PaymentMethod, c.Status, c.CustomerId));//todo CustomerEmail { set; get; }

        CreateMap<BillDetailViewModel, BillDetail>()
              .ConstructUsing(c => new BillDetail(c.Id, c.BillId, c.ProductId,
              c.Quantity, c.Price, c.ColorId, c.SizeId));


            CreateMap<ContactViewModel, Contact>()
                .ConstructUsing(c => new Contact(c.Id, c.Name, c.Phone, c.Email, c.Website, c.Address, c.Other, c.Lng, c.Lat, c.Status));

            CreateMap<FeedbackViewModel, Feedback>()
                .ConstructUsing(c => new Feedback(c.Id, c.Name, c.Email, c.Message, c.Status));

            CreateMap<PageViewModel, Page>()
             .ConstructUsing(c => new Page(c.Id, c.Name, c.Alias, c.Content, c.Status));


           CreateMap<AnnouncementViewModel, Announcement>()
                .ConstructUsing(c => new Announcement(c.Title, c.Content, c.UserId, c.Status));

           CreateMap<AnnouncementUserViewModel, AnnouncementUser>()
                .ConstructUsing(c => new AnnouncementUser(c.AnnouncementId, c.UserId, c.HasRead));
        }
    }
}
