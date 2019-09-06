using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Shop3.Utilities.Extensions
{
    public static class EnumExtensions
    {
        // giúp parse ra giá trị của các  description trong descriptionAttributes 
        // ví dụ trong billstatus  Description("New bill")] New sẽ lấy ra new bill
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = null;

            //if (e is Enum)
            if (e is System.Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                        {
                            // we're only getting the first description we find
                            // others will be ignored
                            description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                        }

                        break;
                    }
                }
            }

            return description;
        }
    }
}
