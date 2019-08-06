using System;
using System.Collections.Generic;
using System.Text;
using Shop3.Application.Implementation;
using Shop3.Application.ViewModels.Common;
using Shop3.Application.ViewModels.Custom;
using Shop3.Utilities.Dtos;

namespace Shop3.Application.Interfaces
{
    public interface ISlideService
    {
        PagedResult<CustomSlideViewModel> GetAllSlidesPagingByGroupAlias( string keyword, int page, int pageSize);
        List<SlideViewModel>  GetAllSlidesByGroupAlias(string alias);
        SlideViewModel GetSlideById(int id);

        void AddSlide(SlideViewModel slideViewModel);

        void UpdateSlide(SlideViewModel slide);

        void DeleteSlideById(int id);
        void DeleteSlideByAlias(string alias);

        void Save();
    }
}
