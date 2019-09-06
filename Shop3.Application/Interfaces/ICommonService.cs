using Shop3.Application.ViewModels.Common;
using System.Collections.Generic;

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
