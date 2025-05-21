using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace E_commerceTechnologyWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly DataContext _context;

        public DashboardController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Chỉ lấy các đơn hàng đã xử lý (Status != 1)
            var processedOrders = await _context.Orders
                .Where(o => o.Status != 1)
                .ToListAsync();

            // Tổng số đơn hàng đã xử lý
            var totalOrders = processedOrders.Count;

            // Lấy chi tiết đơn hàng cho các đơn hàng đã xử lý
            var orderDetails = await _context.OrderDetails
                .Where(od => processedOrders.Select(o => o.Order_Code).Contains(od.OrderCode))
                .ToListAsync();

            // Tổng doanh thu
            var totalRevenue = orderDetails.Sum(od => od.Price * od.Quantity);

            // Số lượng sản phẩm đã bán
            var totalProductsSold = orderDetails.Sum(od => od.Quantity);

            // Số lượng khách hàng (chỉ tính khách hàng có đơn hàng đã xử lý)
            var totalCustomers = processedOrders.Select(o => o.UserName).Distinct().Count();

            // Top 5 sản phẩm bán chạy
            var topProductsQuery = orderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new {
                    ProductId = g.Key,
                    TotalSold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(5)
                .ToList();

            var productIds = topProductsQuery.Select(tp => tp.ProductId).ToList();
            var products = await _context.SanPham
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new { p.Id, p.Name })
                .ToListAsync();

            var topProductsViewModel = topProductsQuery
                .Join(products,
                    tp => tp.ProductId,
                    p => p.Id,
                    (tp, p) => new {
                        ProductName = p.Name,
                        TotalSold = tp.TotalSold
                    })
                .ToList();

            var topProductNames = topProductsViewModel.Select(p => p.ProductName).ToList();
            var topProductSales = topProductsViewModel.Select(p => p.TotalSold).ToList();

            // Doanh thu theo tháng trong năm hiện tại
            var currentYear = DateTime.Now.Year;
            var revenueByMonth = processedOrders
                .Where(o => o.CreateDate.Year == currentYear)
                .Join(orderDetails,
                    o => o.Order_Code,
                    od => od.OrderCode,
                    (o, od) => new { o.CreateDate, Revenue = od.Price * od.Quantity })
                .GroupBy(x => x.CreateDate.Month)
                .Select(g => new { Month = g.Key, Revenue = g.Sum(x => x.Revenue) })
                .OrderBy(x => x.Month)
                .ToList();

            var monthLabels = revenueByMonth.Select(r => "Tháng " + r.Month).ToList();
            var monthlyRevenue = revenueByMonth.Select(r => r.Revenue).ToList();

            // Số lượng sản phẩm còn lại trong kho
            var totalInventory = await _context.SanPham.SumAsync(p => p.Quantity);

            // Số lượng danh mục sản phẩm
            var totalCategories = await _context.TheLoai.CountAsync();

            // Số lượng thương hiệu
            var totalBrands = await _context.ThuongHieu.CountAsync();

            var maxRevenue = monthlyRevenue.Any() ? monthlyRevenue.Max() : 0;
            var maxSold = topProductSales.Any() ? topProductSales.Max() : 0;

            // Đơn hàng gần đây (chỉ lấy 5 đơn hàng đã xử lý gần nhất)
            var recentOrders = await _context.Orders
                .Where(o => o.Status != 1)
                .OrderByDescending(o => o.CreateDate)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalProductsSold = totalProductsSold;
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TopProductNames = topProductNames;
            ViewBag.TopProductSales = topProductSales;
            ViewBag.MonthLabels = monthLabels;
            ViewBag.MonthlyRevenue = monthlyRevenue;
            ViewBag.MaxRevenue = maxRevenue;
            ViewBag.MaxSold = maxSold;
            ViewBag.TotalInventory = totalInventory;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalBrands = totalBrands;
            ViewBag.RecentOrders = recentOrders;

            return View();
        }
    }
}