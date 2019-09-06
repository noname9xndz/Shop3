using Shop3.Application.ViewModels.Blogs;
using Shop3.Utilities.Dtos;
using System.Collections.Generic;

namespace Shop3.Models.BlogViewModels
{
    public class SearchResultViewModel
    {
        public string Keyword { get; set; }
        public List<BlogViewModel> Blog { set; get; }
        public PagedResult<BlogViewModel> BlogList { get; set; }
        public int? pageSize { set; get; }
    }
}
