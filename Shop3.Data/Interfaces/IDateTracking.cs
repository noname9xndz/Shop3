using System;

namespace Shop3.Data.Interfaces
{
    public interface IDateTracking
    {// chứa các thuộc tính dạng datetime dùng chung
        DateTime DateCreated { set; get; } // ngày tạo

        DateTime DateModified { set; get; } // ngày sửa
    }
}
