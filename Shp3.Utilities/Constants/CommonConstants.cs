using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Utilities.Constants
{
    // chứa những biến , keyword dùng chung
    public class CommonConstants
    {
        public const string DefaultFooterId = "DefaultFooterId";


        public const string DefaultContactId = "default";
        
        public const string CartSession = "CartSession";

        public const string ProductTag = "Product";

        public const string BlogTag = "Blog";

        public const string BillCompeleted = "BillCompeleted";



        public class AppRole
        {
            public const string AdminRole = "Admin";
        }
        public class UserClaims
        {
            public const string Roles = "Roles";
        }
        public class PageDefault
        {
            public const string DeliveryInformation = "DeliveryInformation";
            public const string PrivacyPolicy = "PrivacyPolicy";
            public const string FAQ = "FAQ";
            public const string TermsCondition = "TermsCondition";
            public const string ReturnPolicy = "ReturnPolicy";
            public const string About = "About";
        }
    }
}
