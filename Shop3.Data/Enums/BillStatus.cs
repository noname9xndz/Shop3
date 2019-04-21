using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shop3.Data.Enums
{
    public enum BillStatus // thông tin đơn hàng 
    {
        [Description("New bill")] New, //0
        [Description("In Progress")] InProgress,//1
        [Description("Returned")] Returned,//2
        [Description("Cancelled")] Cancelled,//3
        [Description("Completed")] Completed//4

    }
}
