using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace E_commerceTechnologyWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ShippingController : Controller
    {
        private readonly DataContext _dataContext;
        public ShippingController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            var shippingList = await _dataContext.Shipping.ToListAsync();
            ViewBag.Shipping = shippingList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> StoreShipping (ShippingModel shippingModel, string phuong, string quan, string tinh, decimal price)
        {
            shippingModel.Ward = phuong;
            shippingModel.District = quan;
            shippingModel.City = tinh;
            shippingModel.Price = price;

            try
            {
                var existingShipping = await _dataContext.Shipping.AnyAsync(x => x.City == tinh && x.District == quan && x.Ward == phuong);
                // kiểm tra trùng lặp, có hay chưa trong dữ liệu về gói ship này
                if (existingShipping)
                {
                    return Ok(new { duplicate = true, message = "Dữ liệu trùng lặp." });
                }
                _dataContext.Shipping.Add(shippingModel);
                await _dataContext.SaveChangesAsync();
                return Ok(new {success = true, message ="Thêm gói ship thành công."});
            }
            catch (Exception)
            {
                return StatusCode(500, "Có một lỗi khi thêm gói ship, vui lòng thử lại.");
            }
        }
        public async Task<IActionResult> Delete(int Id)
        {
            ShippingModel shipping = await _dataContext.Shipping.FindAsync(Id);

            _dataContext.Shipping.Remove(shipping);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Gói ship đã được xóa thành công.";
            return RedirectToAction("Index","Shipping");
        }
            
    }
}
