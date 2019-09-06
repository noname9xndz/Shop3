using Shop3.DependencyResolver;
using System.ComponentModel.Composition;
using Shop3.Application.Implementation;
using Shop3.Application.Interfaces;
using Shop3.Application.Shared;

namespace Shop3.Application
{
    [Export(typeof(IDependencyResolver))]
    public class ApplicationDependencyResolver : IDependencyResolver
    {
        public void SetUp(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.AddTransient<IProductCategoryService, ProductCategoryService>();
            dependencyRegister.AddTransient<IFunctionService, FunctionService>();
            dependencyRegister.AddTransient<IProductService, ProductService>();
            dependencyRegister.AddTransient<IUserService, UserService>();
            dependencyRegister.AddTransient<IRoleService, RoleService>();
            dependencyRegister.AddTransient<IBillService, BillService>();
            dependencyRegister.AddTransient<ICommonService, CommonService>();
            dependencyRegister.AddTransient<IBlogService, BlogService>();
            dependencyRegister.AddTransient<IContactService, ContactService>();
            dependencyRegister.AddTransient<IFeedbackService, FeedbackService>();
            dependencyRegister.AddTransient<IPageService, PageService>();
            dependencyRegister.AddTransient<IAnnouncementService, AnnouncementService>();
            dependencyRegister.AddTransient<IPageDefaultService, PageDefaultService>();
            dependencyRegister.AddTransient<ISlideService, SlideService>();

            dependencyRegister.AddTransient<IEmailSender, EmailSender>(); 
            dependencyRegister.AddTransient<IViewRenderService, ViewRenderService>();
            dependencyRegister.AddTransient<IFileService, FileService>();
            dependencyRegister.AddTransient<IExcelService, ExcelService>();

        }
    }
}
