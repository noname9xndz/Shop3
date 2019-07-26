using Shop3.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Shop3.Data.Entities;

namespace Shop3.Application.Interfaces
{
    public interface ICommonService
    {
        FooterViewModel GetFooter();
        List<SlideViewModel> GetSlides(string groupAlias);
        SystemConfigViewModel GetSystemConfig(string code);
        PageDefaultViewModel GetPageDefault(string pageDefaultId);
    }
        
}
