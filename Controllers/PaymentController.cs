using E_commerceTechnologyWebsite.Models.Order;
using E_commerceTechnologyWebsite.Services.Momo;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceTechnologyWebsite.Controllers
{
    public class PaymentController : Controller
    {
        private IMomoService _momoService;

        public PaymentController(IMomoService momoService)
        {
            _momoService = momoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMomo(OrderInfoModel model)
        {
            var response = await _momoService.CreatePaymentMomo(model);
            return Redirect(response.PayUrl);
        }

        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }
    }
}
