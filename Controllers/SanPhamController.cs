using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using E_commerceTechnologyWebsite.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceTechnologyWebsite.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly DataContext _dataContext;

        public SanPhamController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Index()
        {
            var sanPhams = await _dataContext.SanPham
                .Include(s => s.TheLoai)
                .Include(s => s.ThuongHieu)
                .ToListAsync();
            return View(sanPhams);
        }

        public async Task<IActionResult> ChiTiet(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var sanphamById = await _dataContext.SanPham
                .Include(p => p.ThuongHieu)
                .Include(p => p.TheLoai)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (sanphamById == null)
            {
                return NotFound();
            }

            var danhGia = await _dataContext.DanhGia
                .Where(d => d.ProductId == sanphamById.Id)
                .OrderByDescending(d => d.Id)
                .ToListAsync();

            var relatedSanPham = await _dataContext.SanPham
                .Where(p => p.ThuongHieuId == sanphamById.ThuongHieuId && p.Id != sanphamById.Id)
                .Take(4)
                .ToListAsync();

            ViewBag.RelatedSanPham = relatedSanPham;

            var viewModel = new DanhGiaViewModel
            {
                SanPhamChiTiet = sanphamById,
                DanhGiaChiTiet = danhGia
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(DanhGiaModel model)
        {
            Console.WriteLine($"Received ProductId: {model.ProductId}");

            if (ModelState.IsValid)
            {
                // Kiểm tra xem sản phẩm có tồn tại không
                var product = await _dataContext.SanPham.FindAsync(model.ProductId);
                if (product == null)
                {
                    Console.WriteLine($"Product with Id {model.ProductId} not found");
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                    return RedirectToAction("Index");
                }

                // Thiết lập thời gian tạo
                model.CreatedDate = DateTime.Now;

                try
                {
                    _dataContext.DanhGia.Add(model);
                    await _dataContext.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Đánh giá của bạn đã được gửi thành công!";
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Error: {ex.InnerException?.Message ?? ex.Message}");
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi gửi đánh giá. Vui lòng thử lại.";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine($"Model validation errors: {string.Join(", ", errors)}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + string.Join(", ", errors);
            }

            return RedirectToAction("ChiTiet", new { id = model.ProductId });
        }

        public async Task<IActionResult> TheLoai(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToAction(nameof(Index));
            }

            var products = await _dataContext.SanPham
                .Where(p => p.TheLoai.Slug == category)
                .Include(s => s.TheLoai)
                .Include(s => s.ThuongHieu)
                .ToListAsync();

            ViewBag.Name = category; // Đặt tên danh mục vào ViewBag để sử dụng trong view
            return View("Index", products); // Sử dụng view Index hiện có
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm, string returnUrl)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Redirect(returnUrl ?? "/");
            }

            var products = await _dataContext.SanPham
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .Include(s => s.TheLoai)
                .Include(s => s.ThuongHieu)
                .ToListAsync();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.ReturnUrl = returnUrl;

            return View(products);
        }
    }
}