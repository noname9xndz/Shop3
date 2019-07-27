using Shop3.Application.ViewModels.Blogs;
using Shop3.Application.ViewModels.Common;
using Shop3.Application.ViewModels.Custom;
using System.Collections.Generic;

namespace Shop3.Models.BlogViewModels
{
    public class BlogDetailViewModel
    {
        public BlogViewModel Blog { get; set; }
        public List<BlogViewModel> GetReatedBlogs { get; set; }
        public List<BlogViewModel> HotBlogs { set; get; }
        public List<SlideViewModel> Slides { get; set; }

        public List<CustomBlogTagViewModel> BlogTag{set;get; }
    }
}
