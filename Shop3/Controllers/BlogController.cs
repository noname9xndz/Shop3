using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shop3.Application.Interfaces;
using Shop3.Models.BlogViewModels;
using System.Threading.Tasks;

namespace Shop3.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private ICommonService _commonService;
        private IConfiguration _configuration;

        public BlogController(IBlogService blogService, ICommonService commonService, IConfiguration configuration)
        {
            _blogService = blogService;
            _commonService = commonService;
            _configuration = configuration;
        }

        [Route("blog.html")]
        public async Task<IActionResult> Index(string keyword, int pageSize, int page = 1)
        {
            var model = new SearchResultViewModel();
            if (pageSize == 0)
                pageSize = _configuration.GetValue<int>("PageSize");
            model.pageSize = pageSize;
            model.BlogList = _blogService.GetAllPaging(keyword, page, pageSize);

            return View(model);
        }

        [Route("{alias}-b.{id}.html", Name = "BlogDetail")]
        public IActionResult BlogDetails(int id)
        {
            var model = new BlogDetailViewModel();
            model.Blog = _blogService.GetById(id);
            model.GetReatedBlogs = _blogService.GetReatedBlogs(id, 8);
            model.HotBlogs = _blogService.GetHotBlog(3);
            model.Slides = _commonService.GetSlides("top");
            model.BlogTag = _blogService.GetBlogWithTagRanDom(7);

            return View(model);
        }
    }
}