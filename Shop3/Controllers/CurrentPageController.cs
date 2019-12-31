using Microsoft.AspNetCore.Mvc;
using Shop3.Application.Interfaces;
using Shop3.Utilities.Constants;

namespace Shop3.Controllers
{
    public class CurrentPageController : Controller
    {
        private readonly ICommonService _commonService;
        public CurrentPageController(ICommonService commonService)
        {
            _commonService = commonService;
        }
        //to do if status = false return view == 404 

        [Route("about.html")]
        public IActionResult About() => View(_commonService.GetPageDefault(CommonConstants.PageDefault.About));

        [Route("faq.html")]
        public IActionResult FAQ() => View(_commonService.GetPageDefault(CommonConstants.PageDefault.FAQ));

        [Route("termscondition.html")]
        public IActionResult TermsCondition() => View(_commonService.GetPageDefault(CommonConstants.PageDefault.TermsCondition));
       
        [Route("returnpolicy.html")]
        public IActionResult ReturnPolicy() => View(_commonService.GetPageDefault(CommonConstants.PageDefault.ReturnPolicy));
        
        [Route("privacypolicy.html")]
        public IActionResult PrivacyPolicy() => View(_commonService.GetPageDefault(CommonConstants.PageDefault.PrivacyPolicy));

        [Route("deliveryinformation.html")]
        public IActionResult DeliveryInformation() => View(_commonService.GetPageDefault(CommonConstants.PageDefault.DeliveryInformation));

    }
}