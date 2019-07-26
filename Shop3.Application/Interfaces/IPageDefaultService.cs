using System;
using System.Collections.Generic;
using System.Text;
using Shop3.Application.ViewModels.Common;

namespace Shop3.Application.Interfaces
{
    public  interface IPageDefaultService
    {
        PageDefaultViewModel GetPageDefault(string pageDefaultId);

        List<PageDefaultViewModel> GetAllPageDefault();

        void Update(PageDefaultViewModel pageVm);

        void SaveChange();
    }
}
