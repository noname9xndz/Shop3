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
            public const string User = "User";
        }
        public class UserClaims
        {
            public const string Roles = "Roles";
            public const string UserId = "UserId";
            public const string Email = "Email";
            public const string UserName = "UserName";
        }
        public class MailSetting
        {
            public const string Server = "MailSettings:Server";
            public const string Port = "MailSettings:Port";
            public const string EnableSsl = "MailSettings:EnableSsl";
            public const string UserName = "MailSettings:UserName";
            public const string Password = "MailSettings:Password";
            public const string FromEmail = "MailSettings:FromEmail";
            public const string FromName = "MailSettings:FromName";

        }

        public class ViewSendMail
        {
           // public const string TaskView = "Task/_EmailAdd";
            //public const string ProjectView = "Project/_SendMailToAdmin";
        }
        public class Permission
        {
            public const string Create = "Create";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string Read = "Read";

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
        public class DbConnection
        {
            public const string connection = "SqlServerConnection";
        }

        public class SortKey
        {
            public const string Lastest = "Lastest";
            public const string Pricelowtohigh = "Pricelowtohigh";
            public const string Pricehightolow = "Pricehightolow";
            public const string NameAZ = "name_az";
            public const string NameZA = "name_za";

        }


    }
}
