using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using E_commerceTechnologyWebsite.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceTechnologyWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DatHangController : Controller
    {
        private readonly DataContext _dataContext;
        public DatHangController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _dataContext.Orders
                .OrderByDescending(p => p.Id)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    Order_Code = o.Order_Code,
                    UserName = o.UserName,
                    CreateDate = o.CreateDate,
                    Status = o.Status
                })
                .ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var order = await _dataContext.Orders
                .FirstOrDefaultAsync(o => o.Order_Code == ordercode);

            if (order == null)
            {
                return NotFound();
            }

            var orderDetails = await _dataContext.OrderDetails
                .Include(p => p.Product)
                .Where(p => p.OrderCode == ordercode)
                .ToListAsync();

            var viewModel = new OrderDetailsViewModel
            {
                Order = order,
                OrderDetails = orderDetails
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(string orderCode, int status)
        {
            try
            {
                var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.Order_Code == orderCode);

                if (order != null)
                {
                    order.Status = status;
                    await _dataContext.SaveChangesAsync();

                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Không tìm thấy đơn hàng." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _dataContext.Orders.FindAsync(id);
            if (order == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn hàng." });
            }

            _dataContext.Orders.Remove(order);
            await _dataContext.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}