using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Application.ViewModels.System
{
    public class VisitorStatisticViewModel
    {
        public int Id { set; get; }
        public string IPAddress { get; set; }

        public DateTime? VisitedDate { set; get; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
