using E_commerceTechnologyWebsite.Controllers; 
using System.IO; // Sử dụng để làm việc với hệ thống file
using Microsoft.AspNetCore.Mvc; 
using E_commerceTechnologyWebsite.KhoLuuTru;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Mvc.Rendering; 
using E_commerceTechnologyWebsite.Models; 
using Microsoft.AspNetCore.Authorization; 

namespace E_commerceTechnologyWebsite.PhanVung.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SanPhamController : Controller // Kế thừa từ Controller
    {
        private readonly DataContext _dataContext; // Biến để lưu trữ DataContext
        private readonly IWebHostEnvironment _webHostEnvironment; // Biến để lưu trữ môi trường web hosting

        public SanPhamController(DataContext context, IWebHostEnvironment webHostEnvironment) // Constructor để inject các dependency
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("Admin")]
        public async Task<IActionResult> Index(int pg = 1)
        {
            const int pageSize = 10; //10 items/trang

            if (pg < 1)
            {
                pg = 1;
            }

            var query = _dataContext.SanPham
                .Include(s => s.TheLoai)
                .Include(s => s.ThuongHieu)
                .OrderByDescending(s => s.Id);

            int totalItems = await query.CountAsync();

            var pager = new PhanTrang(totalItems, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = await query
                .Skip(recSkip)
                .Take(pager.PageSize)
                .ToListAsync();

            ViewBag.Pager = pager;
            ViewBag.TotalProducts = totalItems;

            return View(data);
        }

        [HttpGet]
        public IActionResult Create() // Action Create (GET) để hiển thị form thêm sản phẩm
        {
            // Tạo SelectList cho TheLoai và ThuongHieu để hiển thị list
            ViewBag.TheLoai = new SelectList(_dataContext.TheLoai, "Id", "Name");
            ViewBag.ThuongHieu = new SelectList(_dataContext.ThuongHieu, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Bảo vệ khỏi tấn công Cross-Site Request Forgery (CSRF)
        public async Task<IActionResult> Create(SanPhamModel sanPham) // Action Create (POST) để xử lý thêm sản phẩm
        {
            // Tạo SelectList cho TheLoai và ThuongHieu để hiển thị list (giữ nguyên giá trị đã chọn)
            ViewBag.TheLoai = new SelectList(_dataContext.TheLoai, "Id", "Name", sanPham.TheLoaiId);
            ViewBag.ThuongHieu = new SelectList(_dataContext.ThuongHieu, "Id", "Name", sanPham.ThuongHieuId);

            if (ModelState.IsValid) // Kiểm tra dữ liệu đầu vào có hợp lệ không
            {
                sanPham.Slug = sanPham.Name.Replace(" ", "-"); // Tạo slug từ tên sản phẩm

                // Kiểm tra slug đã tồn tại chưa
                var slug = await _dataContext.SanPham.FirstOrDefaultAsync(p => p.Slug == sanPham.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("Tên sản phẩm", $"Sản phẩm có tên '{sanPham.Name}' đã tồn tại trong hệ thống.");
                    return View(sanPham);
                }

                if (sanPham.ImageUpload != null) // Kiểm tra có file ảnh được upload không
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products"); // Đường dẫn thư mục lưu trữ ảnh
                    string imageName = Guid.NewGuid().ToString() + "_" + sanPham.ImageUpload.FileName; // Tạo tên file ảnh duy nhất
                    string filePath = Path.Combine(uploadDir, imageName); // Đường dẫn đầy đủ của file ảnh

                    FileStream fs = new FileStream(filePath, FileMode.Create); // Tạo file stream để lưu trữ ảnh
                    await sanPham.ImageUpload.CopyToAsync(fs); // Copy nội dung file ảnh vào file stream
                    fs.Close();
                    sanPham.Image = imageName; // Gán tên file ảnh cho sản phẩm
                }

                _dataContext.Add(sanPham); // Thêm sản phẩm vào database
                await _dataContext.SaveChangesAsync(); // Lưu thay đổi vào database
                TempData["success"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            else // Nếu dữ liệu đầu vào không hợp lệ
            {
                TempData["error"] = "Đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage); // Trả về lỗi BadRequest với thông báo lỗi cụ thể
            }
            return View(sanPham);
        }

        public async Task<IActionResult> Edit(long Id) // Action Edit (GET) để hiển thị form sửa sản phẩm
        {
            SanPhamModel sanPham = await _dataContext.SanPham.FindAsync(Id); // Tìm sản phẩm theo Id

            // Tạo SelectList cho TheLoai và ThuongHieu để hiển thị list (giữ nguyên giá trị đã chọn)
            ViewBag.TheLoai = new SelectList(_dataContext.TheLoai, "Id", "Name", sanPham.TheLoaiId);
            ViewBag.ThuongHieu = new SelectList(_dataContext.ThuongHieu, "Id", "Name", sanPham.ThuongHieuId);

            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Bảo vệ khỏi tấn công CSRF
        public async Task<IActionResult> Edit(long Id, SanPhamModel sanPham) // Action Edit (POST) để xử lý sửa sản phẩm
        {
            // Tạo SelectList cho TheLoai và ThuongHieu để hiển thị dropdown list (giữ nguyên giá trị đã chọn)
            ViewBag.TheLoai = new SelectList(_dataContext.TheLoai, "Id", "Name", sanPham.TheLoaiId);
            ViewBag.ThuongHieu = new SelectList(_dataContext.ThuongHieu, "Id", "Name", sanPham.ThuongHieuId);

            var existed_product = _dataContext.SanPham.Find(sanPham.Id); // Tìm sản phẩm theo Id

            if (ModelState.IsValid) // Kiểm tra dữ liệu đầu vào có hợp lệ không
            {
                sanPham.Slug = sanPham.Name.Replace(" ", "-"); // Tạo slug từ tên sản phẩm

                // Kiểm tra slug đã tồn tại chưa (bỏ qua chính sản phẩm đang sửa)
                var slug = await _dataContext.SanPham.FirstOrDefaultAsync(p => p.Slug == sanPham.Slug && p.Id != Id);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(sanPham);
                }

                if (sanPham.ImageUpload != null) // Kiểm tra có file ảnh mới được upload không
                {
                    // up anh moi
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products"); // Đường dẫn thư mục lưu trữ ảnh
                    string imageName = Guid.NewGuid().ToString() + "_" + sanPham.ImageUpload.FileName; // Tạo tên file ảnh duy nhất
                    string filePath = Path.Combine(uploadDir, imageName); // Đường dẫn đầy đủ của file ảnh

                    // xóa ảnh cũ
                    string oldfilePath = Path.Combine(uploadDir, existed_product.Image); // Đường dẫn đầy đủ của file ảnh cũ

                    try
                    {
                        if (System.IO.File.Exists(oldfilePath)) // Kiểm tra file ảnh cũ có tồn tại không
                        {
                            System.IO.File.Delete(oldfilePath); // Xóa file ảnh cũ
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Có một lỗi trong khi đang xóa ảnh cũ");
                    }

                    FileStream fs = new FileStream(filePath, FileMode.Create); // Tạo file stream để lưu trữ ảnh mới
                    await sanPham.ImageUpload.CopyToAsync(fs); // Copy nội dung file ảnh mới vào file stream
                    fs.Close();
                    existed_product.Image = imageName; // Cập nhật tên file ảnh mới cho sản phẩm
                }

                // Cập nhật các thông tin khác của sản phẩm
                existed_product.Name = sanPham.Name;
                existed_product.Description = sanPham.Description;
                existed_product.Price = sanPham.Price;
                existed_product.TheLoaiId = sanPham.TheLoaiId;
                existed_product.ThuongHieuId = sanPham.ThuongHieuId;

                _dataContext.Update(existed_product); // Cập nhật sản phẩm trong database
                await _dataContext.SaveChangesAsync(); // Lưu thay đổi vào database
                TempData["success"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }
            else // Nếu dữ liệu đầu vào không hợp lệ
            {
                TempData["error"] = "Đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage); // Trả về lỗi BadRequest với thông báo lỗi cụ thể
            }
            return View(sanPham);
        }

        public async Task<IActionResult> Delete(long Id)
        {
            var sanPham = await _dataContext.SanPham.FindAsync(Id);
            if (sanPham == null)
            {
                TempData["error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(sanPham.Image) && !string.Equals(sanPham.Image, "noname.jpg"))
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string oldfileImage = Path.Combine(uploadDir, sanPham.Image);

                if (System.IO.File.Exists(oldfileImage))
                {
                    try
                    {
                        System.IO.File.Delete(oldfileImage);
                    }
                    catch (IOException ex)
                    {
                        // Log the error
                        Console.WriteLine($"Error deleting file: {ex.Message}");
                        // Optionally, you can add a message to TempData to inform the user
                        TempData["warning"] = "Không thể xóa file ảnh, nhưng sản phẩm đã được xóa khỏi database";
                    }
                }
            }

            _dataContext.SanPham.Remove(sanPham);
            await _dataContext.SaveChangesAsync();

            TempData["success"] = "Sản phẩm đã xóa thành công";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ToggleActive(long id)
        {
            var sanPham = await _dataContext.SanPham.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            sanPham.IsActive = !sanPham.IsActive;
            await _dataContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //thêm số lượng
        [HttpGet]
        public async Task<IActionResult> ThemSoLuong(long id)
        {
            var sanPham = await _dataContext.SanPham.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound($"Không tìm thấy sản phẩm với ID: {id}");
            }

            var productbyquantity = await _dataContext.SoLuongSanPham.Where(pq => pq.ProductId == id).ToListAsync();

            ViewBag.ProductId = id;
            ViewBag.TenSanPham = sanPham.Name;
            ViewBag.ProductByQuantity = productbyquantity;

            return View(new SoLuongSanPhamModel { ProductId = id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoLuongSanPhamCuaHang(SoLuongSanPhamModel soLuongSanPhamModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ThemSoLuong", soLuongSanPhamModel);
            }

            var product = await _dataContext.SanPham.FindAsync(soLuongSanPhamModel.ProductId);
            if (product == null)
            {
                return NotFound($"Không tìm thấy sản phẩm với ID: {soLuongSanPhamModel.ProductId}");
            }

            product.Quantity += soLuongSanPhamModel.Quantity;
            soLuongSanPhamModel.DateCreated = DateTime.Now;

            _dataContext.SoLuongSanPham.Add(soLuongSanPhamModel);
            await _dataContext.SaveChangesAsync();

            TempData["success"] = "Thêm số lượng sản phẩm thành công";
            return RedirectToAction("Index", "SanPham", new { area = "Admin" });
        }
    }
}