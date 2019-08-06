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
               c.Email, c.PhoneNumber, c.Avatar, c.Status,c.Address,c.Gender));

            CreateMap<PermissionViewModel, Permission>()
             .ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete));
            
            CreateMap<BillViewModel, Bill>()
              .ConstructUsing(c => new Bill(c.Id, c.CustomerName, c.CustomerAddress,
              c.CustomerMobile, c.CustomerMessage, c.BillStatus,
              c.PaymentMethod, c.Status, c.CustomerId, c.ReOrderMesssage,c.CustomerEmail,c.OrderTotal));//todo CustomerEmail { set; get; }

           CreateMap<BillDetailViewModel, BillDetail>()
              .ConstructUsing(c => new BillDetail(c.Id, c.BillId, c.ProductId,
              c.Quantity, c.Price, c.ColorId, c.SizeId));

           CreateMap<WishProductViewModel, WishProduct>()
               .ConstructUsing(c => new WishProduct(c.Id, c.ProductId, c.CustomerId));

            CreateMap<ContactViewModel, Contact>()
                .ConstructUsing(c => new Contact(c.Id, c.Name, c.Phone, c.Email, c.Website, c.Address, c.Other, c.Lng, c.Lat, c.Status));

            CreateMap<FeedbackViewModel, Feedback>()
                .ConstructUsing(c => new Feedback(c.Id, c.Name, c.Email, c.Message, c.Status));

            CreateMap<PageViewModel, Page>()
             .ConstructUsing(c => new Page(c.Id, c.Name, c.Alias, c.Content, c.Status));
            
            CreateMap<BlogViewModel, Blog>()
                .ConstructUsing(c => new Blog(c.Id, c.Name,c.Image,c.Description, c.Content,
                    c.HomeFlag,c.HotFlag,c.Tags, c.Status,c.SeoPageTitle,c.SeoAlias,c.SeoKeywords, c.SeoDescription));


            CreateMap<AnnouncementViewModel, Announcement>()
                .ConstructUsing(c => new Announcement(c.Title, c.Content, c.UserId, c.Status));

           CreateMap<AnnouncementUserViewModel, AnnouncementUser>()
                .ConstructUsing(c => new AnnouncementUser(c.AnnouncementId, c.UserId, c.HasRead));

           CreateMap<PageDefaultViewModel, PageDefault>()
               .ConstructUsing(c => new PageDefault(c.Id, c.Title, c.Content, c.Status));

           CreateMap<PageDefaultViewModel, PageDefault>()
               .ConstructUsing(c => new PageDefault(c.Id, c.Title, c.Content, c.Status));

           CreateMap<SupportOnlineViewModel, SupportOnline>()
               .ConstructUsing(c => new SupportOnline(c.Id, c.Name,c.Skype,c.FaceBook,c.Yahoo,c.Pinterest,c.Twitter,c.Google,c.Mobile,
                   c.Email,c.Instagram,c.Youtube,c.Linkedin,c.Zalo,c.TimeOpenWindow,c.Other,
                   c.Status,c.DateCreated,c.DateModified,c.DisplayOrder));

           CreateMap<SlideViewModel, Slide>()
               .ConstructUsing(c => new Slide(c.Id,c.Name,c.Description,c.Image,c.Url,c.DisplayOrder,c.Status,c.Content,c.GroupAlias));

            CreateMap<QuestionViewModel,Question>()
               .ConstructUsing(c => new Question(c.Id, c.Title, c.Content, c.DisplayOrder,c.Status));

            //CreateMap<ErrorViewModel, Error>()
            //    .ConstructUsing(c => new Error(c.Message, c.StackTrace));
        }
    }
}
