using Shop3.DependencyResolver;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Shop3.Application
{
    [Export(typeof(IDependencyResolver))]
    public class ApplicationDependencyResolver : IDependencyResolver
    {
        public void SetUp(IDependencyRegister dependencyRegister)
        {
            //dependencyRegister.AddScoped<IDummyService1, DummyService1>();
            //services.AddTransient<IProductCategoryService, ProductCategoryService>();
            //services.AddTransient<IFunctionService, FunctionService>();
            //services.AddTransient<IProductService, ProductService>();
            //services.AddTransient<IUserService, UserService>();
            //services.AddTransient<IRoleService, RoleService>();
            //services.AddTransient<IBillService, BillService>();
            //services.AddTransient<ICommonService, CommonService>();
            //services.AddTransient<IBlogService, BlogService>();
            //services.AddTransient<IContactService, ContactService>();
            //services.AddTransient<IFeedbackService, FeedbackService>();
            //services.AddTransient<IPageService, PageService>();
            //services.AddTransient<IReportService, ReportService>();
            //services.AddTransient<IAnnouncementService, AnnouncementService>();
            //services.AddTransient<IPageDefaultService, PageDefaultService>();
            //services.AddTransient<ISlideService, SlideService>();
        }
    }
}
