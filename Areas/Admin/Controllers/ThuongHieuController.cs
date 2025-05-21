using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceTechnologyWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Publisher,Author,Admin")]
    [Authorize]
    public class ThuongHieuController : Controller
    {
        private readonly DataContext _dataContext;
        public ThuongHieuController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Index(int pg = 1)
        {
            const int pageSize = 10;

            if (pg < 1)
            {
                pg = 1;
            }

            var query = _dataContext.ThuongHieu
                .OrderByDescending(b => b.Id);

            int totalItems = await query.CountAsync();

            var pager = new PhanTrang(totalItems, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = await query
                .Skip(recSkip)
                .Take(pager.PageSize)
                .ToListAsync();

            ViewBag.Pager = pager;
            ViewBag.TotalBrands = totalItems;

            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThuongHieuModel thuongHieu)
        {
            if (ModelState.IsValid)
            {
                thuongHieu.Slug = thuongHieu.Name.ToLower().Replace(" ", "-");
                var slug = await _dataContext.ThuongHieu.FirstOrDefaultAsync(p => p.Slug == thuongHieu.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("Name", $"Thương hiệu có tên '{thuongHieu.Name}' đã tồn tại trong hệ thống.");
                    return View(thuongHieu);
                }

                _dataContext.Add(thuongHieu);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thương hiệu thành công";
                return RedirectToAction("Index");
            }
            return View(thuongHieu);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var thuongHieu = await _dataContext.ThuongHieu.FindAsync(id);
            if (thuongHieu == null)
            {
                return NotFound();
            }
            return View(thuongHieu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ThuongHieuModel thuongHieu)
        {
            if (id != thuongHieu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    thuongHieu.Slug = thuongHieu.Name.ToLower().Replace(" ", "-");
                    var existingBrand = await _dataContext.ThuongHieu
                        .FirstOrDefaultAsync(p => p.Slug == thuongHieu.Slug && p.Id != thuongHieu.Id);
                    if (existingBrand != null)
                    {
                        ModelState.AddModelError("Name", $"Thương hiệu có tên '{thuongHieu.Name}' đã tồn tại trong hệ thống.");
                        return View(thuongHieu);
                    }

                    _dataContext.Update(thuongHieu);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Cập nhật thương hiệu thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThuongHieuExists(thuongHieu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(thuongHieu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thuongHieu = await _dataContext.ThuongHieu.FindAsync(id);
            if (thuongHieu == null)
            {
                return NotFound();
            }

            _dataContext.ThuongHieu.Remove(thuongHieu);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Thương hiệu đã xóa thành công";
            return RedirectToAction(nameof(Index));
        }

        private bool ThuongHieuExists(int id)
        {
            return _dataContext.ThuongHieu.Any(e => e.Id == id);
        }
    }
}