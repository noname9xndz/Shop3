using System;
using System.Collections.Generic;
using System.Text;

namespace Shp3.Utilities.Dtos
{
    public abstract class PagedResultBase // phân trang
    {
        public int CurrentPage { get; set; } // trang hiện tại

        public int PageCount // tổng số trang
        {
            get
            {
                var pageCount = (double)RowCount / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
            set { PageCount = value; }
        }
        public int PageSize { get; set; }  // kích thước trang (số bản ghi trong 1 trang)

        public int RowCount { get; set; } // tổng số bản ghi

        public int FirstRowOnPage // dòng đầu tiên của trang
        {
            get
            {
                return (CurrentPage - 1) * PageSize + 1;
            }
        }
        public int LastRowOnPage // dòng cuối cùng của trang
        {
            get
            {
                return Math.Min(CurrentPage * PageSize, RowCount);
            }
        }
    }
}
