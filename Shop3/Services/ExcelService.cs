using OfficeOpenXml;
using OfficeOpenXml.Table;
using Shop3.Application.ViewModels.Products;
using System.Collections.Generic;
using System.IO;

namespace Shop3.Services
{
    public class ExcelService : IExcelService
    {
        public void WriteExcel(FileInfo file, List<ProductViewModel> products)
        {
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");
                worksheet.Cells["A1"].LoadFromCollection(products, true, TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
        }
    }
}
