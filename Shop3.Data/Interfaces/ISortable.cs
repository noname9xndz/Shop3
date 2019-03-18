using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Data.Interfaces
{
    public interface ISortable
    {
        int SortOrder { set; get; } // thứ tự sắp xếp
    }
}
