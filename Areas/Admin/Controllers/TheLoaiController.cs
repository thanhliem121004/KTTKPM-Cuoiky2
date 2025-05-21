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
    public class TheLoaiController : Controller
    {
        private readonly DataContext _dataContext;
        public TheLoaiController(DataContext context)
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

            var query = _dataContext.TheLoai
                .OrderByDescending(c => c.Id);

            int totalItems = await query.CountAsync();

            var pager = new PhanTrang(totalItems, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = await query
                .Skip(recSkip)
                .Take(pager.PageSize)
                .ToListAsync();

            ViewBag.Pager = pager;
            ViewBag.TotalCategories = totalItems;

            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TheLoaiModel theLoai)
        {
            if (ModelState.IsValid)
            {
                theLoai.Slug = theLoai.Name.ToLower().Replace(" ", "-");
                var slug = await _dataContext.TheLoai.FirstOrDefaultAsync(p => p.Slug == theLoai.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("Name", $"Thể loại có tên '{theLoai.Name}' đã tồn tại trong hệ thống.");
                    return View(theLoai);
                }

                _dataContext.Add(theLoai);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thể loại thành công";
                return RedirectToAction("Index");
            }
            return View(theLoai);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var theLoai = await _dataContext.TheLoai.FindAsync(id);
            if (theLoai == null)
            {
                return NotFound();
            }
            return View(theLoai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TheLoaiModel theLoai)
        {
            if (id != theLoai.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    theLoai.Slug = theLoai.Name.ToLower().Replace(" ", "-");
                    var existingCategory = await _dataContext.TheLoai
                        .FirstOrDefaultAsync(p => p.Slug == theLoai.Slug && p.Id != theLoai.Id);
                    if (existingCategory != null)
                    {
                        ModelState.AddModelError("Name", $"Thể loại có tên '{theLoai.Name}' đã tồn tại trong hệ thống.");
                        return View(theLoai);
                    }

                    _dataContext.Update(theLoai);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Cập nhật thể loại thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheLoaiExists(theLoai.Id))
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
            return View(theLoai);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var theLoai = await _dataContext.TheLoai.FindAsync(id);
            if (theLoai == null)
            {
                return NotFound();
            }

            _dataContext.TheLoai.Remove(theLoai);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Thể loại đã xóa thành công";
            return RedirectToAction(nameof(Index));
        }

        private bool TheLoaiExists(int id)
        {
            return _dataContext.TheLoai.Any(e => e.Id == id);
        }
    }
}