using Shop3.Application.ViewModels.Products;
using System.Collections.Generic;
using System.IO;

namespace Shop3.Application.Shared
{
    public interface IExcelService
    {
        void WriteExcel(FileInfo file, List<ProductViewModel> products);

    }
}

