using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace E_commerceTechnologyWebsite.Controllers
{
    public class ThuongHieuController : Controller 
    {
        private readonly DataContext _dataContext;
        public ThuongHieuController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(string Slug = "", string sort_by ="")
        {
            ThuongHieuModel thuongHieu = _dataContext.ThuongHieu.Where(c => c.Slug == Slug).FirstOrDefault();
            if (thuongHieu == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Name = thuongHieu.Name;

            IQueryable<SanPhamModel> sanphamBythuongHieu = _dataContext.SanPham.Where(p => p.ThuongHieuId == thuongHieu.Id);
            var count = await sanphamBythuongHieu.CountAsync();
            if (count > 0)
            {
                if (sort_by == "price_increase")
                {
                    sanphamBythuongHieu = sanphamBythuongHieu.OrderBy(p => p.Price);
                }
                else if (sort_by == "price_decrease")
                {
                    sanphamBythuongHieu = sanphamBythuongHieu.OrderByDescending(p => p.Price);
                }
                else if (sort_by == "price_newest")
                {
                    sanphamBythuongHieu = sanphamBythuongHieu.OrderByDescending(p => p.Id);
                }
                else if (sort_by == "price_oldest")
                {
                    sanphamBythuongHieu = sanphamBythuongHieu.OrderBy(p => p.Id);
                }
                else
                {
                    sanphamBythuongHieu = sanphamBythuongHieu.OrderByDescending(p => p.Id);
                }
            }
            return View(await sanphamBythuongHieu.ToListAsync());
        }
    }
}
