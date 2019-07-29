using Shop3.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Application.ViewModels.Common
{
    public class SystemConfigViewModel
    {
        public string Id { set; get; }
        public string Name { get; set; }

        public string StringValue { get; set; }
        public int? IntValue { get; set; }

        public bool? BoolValue { get; set; }

        public DateTime? DateTimeValue { get; set; }

        public decimal? DecimalValue { get; set; }
        public Status Status { get; set; }
    }
}
