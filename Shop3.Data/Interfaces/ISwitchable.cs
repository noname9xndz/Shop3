using Shop3.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Data.Interfaces
{
    public interface ISwitchable
    {// thuộc  tính ,đối tượng đã được kích hoạt hay ko???
        Status Status { set; get; }
    }
}
