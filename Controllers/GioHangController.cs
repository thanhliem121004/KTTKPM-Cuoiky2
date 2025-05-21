using E_commerceTechnologyWebsite.KhoLuuTru;
using Microsoft.AspNetCore.Mvc;
using E_commerceTechnologyWebsite.Models;
using E_commerceTechnologyWebsite.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Net;

namespace E_commerceTechnologyWebsite.Controllers
{
    public class GioHangController : Controller
    {
        private readonly DataContext _dataContext;

        public GioHangController(DataContext _context)
        {
            _dataContext = _context;
        }

        public async Task<IActionResult> Index()
        {
            List<GioHangModel> gioHangItem = HttpContext.Session.GetJson<List<GioHangModel>>("GioHang") ?? new List<GioHangModel>();
            var coupon_code = Request.Cookies["CouponTitle"];
            // Kiểm tra và loại bỏ các sản phẩm không còn hoạt động
            var activeProductIds = await _dataContext.SanPham
                .Where(p => p.IsActive)
                .Select(p => (long)p.Id)
                .ToListAsync();

            gioHangItem = gioHangItem.Where(item => activeProductIds.Contains(item.ProductId)).ToList();

            // Cập nhật lại session sau khi lọc
            HttpContext.Session.SetJson("GioHang", gioHangItem);

            var gioHangVM = new GioHangViewModel
            {
                GioHang = gioHangItem,
                TongTien = gioHangItem.Sum(x => x.Quantity * x.Price),
                CouponCode = coupon_code,
            };

            return View(gioHangVM);
        }

        public IActionResult ThanhToan()
        {
            return View("~/Views/ThanhToan/Index.cshtml");
        }

        public async Task<IActionResult> Them(long Id, int Quantity = 1)
        {
            var sanPham = await _dataContext.SanPham.FindAsync(Id);
            if (sanPham == null || !sanPham.IsActive)
            {
                TempData["error"] = "Sản phẩm không tồn tại hoặc đã ngừng hoạt động";
                return Json(new { success = false, message = TempData["error"] });
            }

            List<GioHangModel> gioHang = HttpContext.Session.GetJson<List<GioHangModel>>("GioHang") ?? new List<GioHangModel>();
            GioHangModel gioHangItem = gioHang.FirstOrDefault(c => c.ProductId == Id);
            if (gioHangItem == null)
            {
                gioHangItem = new GioHangModel(sanPham);
                gioHangItem.Quantity = Quantity;
                gioHang.Add(gioHangItem);
            }
            else
            {
                gioHangItem.Quantity += Quantity;
            }

            HttpContext.Session.SetJson("GioHang", gioHang);
            TempData["success"] = "Thêm sản phẩm vào giỏ hàng thành công";
            return Json(new { success = true, message = TempData["success"] });
        }

        public async Task<IActionResult> Giam(int Id)
        {
            List<GioHangModel> gioHang = HttpContext.Session.GetJson<List<GioHangModel>>("GioHang");

            GioHangModel gioHangItem = gioHang.FirstOrDefault(c => c.ProductId == Id);
            if (gioHangItem != null)
            {
                if (gioHangItem.Quantity > 1)
                {
                    --gioHangItem.Quantity;
                }
                else
                {
                    gioHang.RemoveAll(c => c.ProductId == Id);
                }

                if (gioHang.Count == 0)
                {
                    HttpContext.Session.Remove("GioHang");
                }
                else
                {
                    HttpContext.Session.SetJson("GioHang", gioHang);
                }

                TempData["success"] = "Giảm số lượng sản phẩm thành công";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Tang(long Id)
        {
            SanPhamModel sanpham = await _dataContext.SanPham.Where(c => c.Id == Id).FirstOrDefaultAsync();

            List<GioHangModel> gioHang = HttpContext.Session.GetJson<List<GioHangModel>>("GioHang");

            GioHangModel gioHangItem = gioHang.FirstOrDefault(c => c.ProductId == Id);
            if (gioHangItem.Quantity >= 1 && sanpham.Quantity > gioHangItem.Quantity)
            {
                ++gioHangItem.Quantity;
                TempData["success"] = "Tăng số lượng thành công";
            }
            else
            {
                gioHangItem.Quantity = sanpham.Quantity;
                TempData["success"] = "Đã tối đa số lượng sản phẩm có trong cửa hàng";
            }
            if (gioHang.Count == 0)
            {
                HttpContext.Session.Remove("GioHang");
            }
            else
            {
                HttpContext.Session.SetJson("GioHang", gioHang);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Xoa(int Id)
        {
            List<GioHangModel> gioHang = HttpContext.Session.GetJson<List<GioHangModel>>("GioHang");

            gioHang.RemoveAll(c => c.ProductId == Id);

            if (gioHang.Count == 0)
            {
                HttpContext.Session.Remove("GioHang");
            }
            else
            {
                HttpContext.Session.SetJson("GioHang", gioHang);
            }

            TempData["success"] = "Xóa sản phẩm khỏi giỏ hàng thành công";
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("GioHang");
            TempData["success"] = "Xóa tất cả sản phẩm thành công";
            return RedirectToAction("Index");
        }

        private async Task RemoveInactiveProducts()
        {
            List<GioHangModel> gioHang = HttpContext.Session.GetJson<List<GioHangModel>>("GioHang");
            if (gioHang != null)
            {
                var activeProductIds = await _dataContext.SanPham
                    .Where(p => p.IsActive)
                    .Select(p => (long)p.Id)  // Chuyển đổi Id thành long
                    .ToListAsync();

                gioHang = gioHang.Where(item => activeProductIds.Contains(item.ProductId)).ToList();
                HttpContext.Session.SetJson("GioHang", gioHang);
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetShipping(string tinh, string quan, string phuong)
        {
            var existingShipping = await _dataContext.Shipping
                .FirstOrDefaultAsync(x => x.City == tinh && x.District == quan && x.Ward == phuong);

            decimal shippingPrice = existingShipping?.Price ?? 50000;

            // Lưu phí vận chuyển vào cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                Secure = true,
                SameSite = SameSiteMode.Lax
            };
            Response.Cookies.Append("ShippingPrice", shippingPrice.ToString(), cookieOptions);

            return Json(new { shippingPrice });
        }
        // hàm lấy mã giảm
        [HttpPost]
        public async Task<IActionResult> GetCoupon(CouponModel couponModel, string coupon_value)
        {
            var validCoupon = await _dataContext.Coupon.FirstOrDefaultAsync(x => x.Name == coupon_value);
            string couponTitle = validCoupon.Name + " | " + validCoupon?.Description;

            if (couponTitle != null)
            {
                TimeSpan remainingTime = validCoupon.DayExpired - DateTime.Now;
                int daysRemaining = remainingTime.Days;

                if (daysRemaining >= 0)
                {
                    try
                    {
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        };
                        Response.Cookies.Append("CouponTitle", couponTitle, cookieOptions);
                        return Ok(new { succuess = true, message = "Sử dụng mã giảm thành công" });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Có lỗi Cookie khi sử dụng mã giảm này: {ex.Message}");
                        return Ok(new { success = false, message = "Sử dụng mã giảm thất bại" });
                    }
                }
                else
                {
                    return Ok(new { success = false, message = "Mã giảm đã hết hạn" });
                }
            }
            else
            {
                return Ok(new { success = false, message = "Mã giảm không tồn tại" });
            }
            return Json(new { CouponTitle = couponTitle });
        }
        //public async Task<IActionResult> GetCoupon(string coupon_value)
        //{
        //    if (string.IsNullOrWhiteSpace(coupon_value))
        //    {
        //        return Ok(new { success = false, message = "Vui lòng nhập mã giảm giá" });
        //    }

        //    var validCoupon = await _dataContext.Coupon.FirstOrDefaultAsync(x => x.Name == coupon_value);

        //    if (validCoupon == null)
        //    {
        //        return Ok(new { success = false, message = "Mã giảm giá không tồn tại" });
        //    }

        //    if (validCoupon.DayExpired < DateTime.Now)
        //    {
        //        return Ok(new { success = false, message = "Mã giảm giá đã hết hạn" });
        //    }

        //    string couponTitle = $"{validCoupon.Name} | {validCoupon.Description}";

        //    try
        //    {
        //        var cookieOptions = new CookieOptions
        //        {
        //            HttpOnly = true,
        //            Expires = DateTimeOffset.UtcNow.AddMinutes(30),
        //            Secure = true,
        //            SameSite = SameSiteMode.Strict
        //        };
        //        Response.Cookies.Append("CouponTitle", couponTitle, cookieOptions);
        //        return Ok(new { success = true, message = "Sử dụng mã giảm giá thành công", couponTitle = couponTitle });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        Console.WriteLine($"Có lỗi Cookie khi sử dụng mã giảm này: {ex.Message}");
        //        return Ok(new { success = false, message = "Sử dụng mã giảm thất bại" });
        //    }
        //}
    }
}