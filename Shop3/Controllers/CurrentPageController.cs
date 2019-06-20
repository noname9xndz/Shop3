using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Shop3.Controllers
{
    public class CurrentPageController : Controller
    {
        [Route("about.html")]
        public IActionResult About()
        {
            return View();
        }

        [Route("faq.html")]
        public IActionResult FAQ()
        {
            return View();
        }
        [Route("termscondition.html")]
        public IActionResult TermsCondition()
        {
            return View();
        }
        [Route("returnpolicy.html")]
        public IActionResult ReturnPolicy()
        {
            return View();
        }
        [Route("privacypolicy.html")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        [Route("deliveryinformation.html")]
        public IActionResult DeliveryInformation()
        {
            return View();
        }







    }
}