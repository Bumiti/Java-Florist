using JavaFloristClient.Models.VnPayPayments;
using JavaFloristClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace JavaFloristClient.Controllers
{
    public class VnPayController : Controller
    {
        private readonly IVnPayService _vnPayService;

        public VnPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        public IActionResult Index(string name, double amount)
        {
            ViewBag.PaymentName = name;
            ViewBag.PaymentAmount = Math.Round(amount * 23500, 2);
            return View();
        }

        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response != null)
            {
                HttpContext.Session.SetString("CheckOut", "Success");
                return RedirectToAction("CheckOut", "Customers");
            }
            return Json(response);
        }
    }
}
