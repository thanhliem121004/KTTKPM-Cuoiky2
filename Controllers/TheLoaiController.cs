using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceTechnologyWebsite.Controllers
{
    public class TheLoaiController : Controller
    {
        private readonly DataContext _dataContext;
        public TheLoaiController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(string Slug = "", string sort_by = "")
        {
            TheLoaiModel theloai = _dataContext.TheLoai.Where(c => c.Slug == Slug).FirstOrDefault();
            if (theloai == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Name = theloai.Name;

            IQueryable<SanPhamModel> sanphamBytheloai = _dataContext.SanPham.Where(p => p.TheLoaiId == theloai.Id);
            var count = await sanphamBytheloai.CountAsync();

            if (count > 0)
            {
                if (sort_by == "price_increase")
                {
                    sanphamBytheloai = sanphamBytheloai.OrderBy(p=>p.Price);
                }    
                else if (sort_by == "price_decrease")
                {
                    sanphamBytheloai = sanphamBytheloai.OrderByDescending(p=>p.Price);
                }
                else if (sort_by == "price_newest")
                {
                    sanphamBytheloai = sanphamBytheloai.OrderByDescending(p=>p.Id);
                }
                else if (sort_by == "price_oldest")
                {
                    sanphamBytheloai = sanphamBytheloai.OrderBy(p => p.Id);
                }
                else
                {
                    sanphamBytheloai = sanphamBytheloai.OrderByDescending(p => p.Id);
                }
            } 
                

            return View(await sanphamBytheloai.ToListAsync());
        }
    }
}
