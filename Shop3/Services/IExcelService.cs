using Shop3.Application.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shop3.Services
{
    public interface IExcelService
    {
        void WriteExcel(FileInfo file, List<ProductViewModel> products);
    }
}

