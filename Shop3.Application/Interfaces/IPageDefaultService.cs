using Shop3.Application.ViewModels.Common;
using System.Collections.Generic;

namespace Shop3.Application.Interfaces
{
    public interface IPageDefaultService
    {
        PageDefaultViewModel GetPageDefault(string pageDefaultId);

        List<PageDefaultViewModel> GetAllPageDefault();

        void Update(PageDefaultViewModel pageVm);

        void SaveChange();
    }
}
