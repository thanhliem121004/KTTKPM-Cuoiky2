using E_commerceTechnologyWebsite.Areas.Admin.KhoLuuTru;
using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using E_commerceTechnologyWebsite.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace E_commerceTechnologyWebsite.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ThanhToanController> _logger;

        public ThanhToanController(DataContext context, IEmailSender emailSender, ILogger<ThanhToanController> logger)
        {
            _dataContext = context;
            _emailSender = emailSender;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetJson<List<GioHangModel>>("GioHang") ?? new List<GioHangModel>();
            decimal shippingFee = 0;

            if (Request.Cookies.TryGetValue("ShippingPrice", out string shippingPriceString))
            {
                if (decimal.TryParse(shippingPriceString, out decimal parsedShippingFee))
                {
                    shippingFee = parsedShippingFee;
                }
            }

            var viewModel = new ThanhToanViewModel
            {
                CartItems = cart,
                TotalAmount = cart.Sum(item => item.Quantity * item.Price),
                ShippingFee = shippingFee,
                GrandTotal = cart.Sum(item => item.Quantity * item.Price) + shippingFee
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ThanhToanViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CartItems = HttpContext.Session.GetJson<List<GioHangModel>>("GioHang") ?? new List<GioHangModel>();
                    if (model.CartItems.Count == 0)
                    {
                        ModelState.AddModelError("", "Giỏ hàng của bạn đang trống.");
                        return View(model);
                    }

                    model.TotalAmount = model.CartItems.Sum(item => item.Quantity * item.Price);
                    var coupon_code = Request.Cookies["CouponTitle"];
                    var orderCode = Guid.NewGuid().ToString();

                    var orderItem = new OrderModel
                    {
                        Order_Code = orderCode,
                        UserName = model.Email,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        Status = 1,
                        CreateDate = DateTime.Now,
                        CouponCode = coupon_code,
                    };
                    _dataContext.Add(orderItem);
                    await _dataContext.SaveChangesAsync();

                    foreach (var gioHang in model.CartItems)
                    {
                        var orderdetails = new OrderDetails
                        {
                            UserName = model.Email,
                            OrderCode = orderCode,
                            ProductId = gioHang.ProductId,
                            Price = gioHang.Price,
                            Quantity = gioHang.Quantity
                        };
                        var product = await _dataContext.SanPham.Where(p => p.Id == gioHang.ProductId).FirstAsync();
                        product.Quantity -= gioHang.Quantity;
                        product.Sold += gioHang.Quantity;
                        _dataContext.Update(product);
                        _dataContext.Add(orderdetails);
                    }
                    await _dataContext.SaveChangesAsync();

                    HttpContext.Session.Remove("GioHang");

                    var receiver = model.Email;
                    var subject = "Đơn hàng đã được tạo thành công! Cảm ơn bạn đã tin tưởng";
                    var message = $"Xin chào {model.FullName},\n\nĐơn hàng của bạn đã được đặt thành công. Chúng tôi sẽ giao hàng đến địa chỉ: {model.Address}\n\nCảm ơn bạn đã mua hàng!";

                    await _emailSender.SendEmailAsync(receiver, subject, message);
                    TempData["success"] = "Đơn hàng đã được tạo thành công";
                    _logger.LogInformation("Đơn hàng đã được tạo thành công. Chuyển hướng đến GioHang/Index");
                    return RedirectToAction("Index", "GioHang");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý đơn hàng");
                ModelState.AddModelError("", "Có lỗi xảy ra khi xử lý đơn hàng: " + ex.Message);
            }

            _logger.LogInformation("Hiển thị lại form thanh toán do lỗi");
            model.CartItems = HttpContext.Session.GetJson<List<GioHangModel>>("GioHang") ?? new List<GioHangModel>();
            model.TotalAmount = model.CartItems.Sum(item => item.Quantity * item.Price);
            return View(model);
        }

        public IActionResult XacNhanDonHang(string orderCode)
        {
            var order = _dataContext.Orders.FirstOrDefault(o => o.Order_Code == orderCode);
            if (order == null)
            {
                return NotFound();
            }
            var orderDetails = _dataContext.OrderDetails.Where(od => od.OrderCode == orderCode).ToList();
            var viewModel = new XacNhanDonHangViewModel
            {
                Order = order,
                OrderDetails = orderDetails
            };
            return View(viewModel);
        }
    }
}