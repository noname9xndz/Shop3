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
        public IActionResult About()
        {

            var model = _commonService.GetPageDefault(CommonConstants.PageDefault.About);
            return View(model);
        }

        [Route("faq.html")]
        public IActionResult FAQ()
        {
            var model = _commonService.GetPageDefault(CommonConstants.PageDefault.FAQ);
            return View(model);
        }
        [Route("termscondition.html")]
        public IActionResult TermsCondition()
        {
            var model = _commonService.GetPageDefault(CommonConstants.PageDefault.TermsCondition);
            return View(model);
        }
        [Route("returnpolicy.html")]
        public IActionResult ReturnPolicy()
        {
            var model = _commonService.GetPageDefault(CommonConstants.PageDefault.ReturnPolicy);
            return View(model);
        }
        [Route("privacypolicy.html")]
        public IActionResult PrivacyPolicy()
        {
            var model = _commonService.GetPageDefault(CommonConstants.PageDefault.PrivacyPolicy);
            return View(model);
        }
        [Route("deliveryinformation.html")]
        public IActionResult DeliveryInformation()
        {
            var model = _commonService.GetPageDefault(CommonConstants.PageDefault.DeliveryInformation);
            return View(model);
        }

    }
}