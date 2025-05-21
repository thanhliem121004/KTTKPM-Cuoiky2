    using E_commerceTechnologyWebsite.Controllers;
    using E_commerceTechnologyWebsite.KhoLuuTru;
    using E_commerceTechnologyWebsite.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore; // Thêm namespace này

    namespace E_commerceTechnologyWebsite.Areas.Admin.Controllers
    {
        [Area("Admin")]
        [Route("Admin/User")]
        //[Authorize(Roles = "Admin")]
         [Authorize]

    public class UserController : Controller
        {
            private readonly UserManager<AppUserModel> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private void AddIdentityErrors(IdentityResult result)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

        [Route("Index")]
        public async Task<IActionResult> Index(int pg = 1)
        {
            const int pageSize = 10;
            if (pg < 1) pg = 1;

            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userViewModel = new UserViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Password = user.PasswordHash,
                    Role = roles.FirstOrDefault(),
                    LockoutEndTime = user.LockoutEndTime
                };

                userViewModels.Add(userViewModel);
            }

            int recsCount = userViewModels.Count();
            var pager = new PhanTrang(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;

            var data = userViewModels.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
        }
            [HttpGet]
            [Route("Create")]
            public async Task<IActionResult> Create()
            {
                var roles = await _roleManager.Roles.ToListAsync();
                ViewBag.Roles = new SelectList(roles,"Id","Name");
                return View(new AppUserModel());
            }

            [HttpGet]
            [Route("Edit/{id}")]
            public async Task<IActionResult> Edit(string id)
            {
                if (string.IsNullOrEmpty(id))
                {
                    return NotFound();
                }
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                var roles = await _roleManager.Roles.ToListAsync();
                ViewBag.Roles = new SelectList(roles, "Id", "Name");
                return View(user);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [Route("Edit/{id}")]
            public async Task<IActionResult> Edit(string id, AppUserModel model)
            {
                if (id != model.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật thông tin người dùng
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    // Cập nhật các trường khác nếu cần

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["success"] = "User đã được cập nhật thành công";
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                // Nếu có lỗi, hiển thị form lại
                var roles = await _roleManager.Roles.ToListAsync();
                ViewBag.Roles = new SelectList(roles, "Id", "Name", model.RoleId);
                return View(model);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [Route("Create")]
            public async Task<IActionResult> Create(AppUserModel user)
            {
                if (ModelState.IsValid)
                {
                    // Tạo user mới với thông tin được cung cấp
                    var createUserResult = await _userManager.CreateAsync(user, user.PasswordHash);
                    if (createUserResult.Succeeded)
                    {
                        // Tìm user vừa tạo dựa trên email
                        var createdUser = await _userManager.FindByEmailAsync(user.Email);

                        // Kiểm tra xem user có tồn tại và có RoleId được cung cấp không
                        if (createdUser != null && !string.IsNullOrEmpty(user.RoleId))
                        {
                            // Tìm role dựa trên RoleId
                            var role = await _roleManager.FindByIdAsync(user.RoleId);
                            if (role != null)
                            {
                                // Thêm user vào role
                                var addToRoleResult = await _userManager.AddToRoleAsync(createdUser, role.Name);
                                if (addToRoleResult.Succeeded)
                                {
                                    // Nếu thêm role thành công, chuyển hướng về trang Index với thông báo thành công
                                    TempData["success"] = "User đã được tạo và gán quyền thành công";
                                    return RedirectToAction("Index", "User");
                                }
                                else
                                {
                                    // Nếu thêm role thất bại, thêm các lỗi vào ModelState
                                    AddIdentityErrors(addToRoleResult);
                                }
                            }
                            else
                            {
                                // Nếu không tìm thấy role, thêm lỗi vào ModelState
                                ModelState.AddModelError(string.Empty, "Không tìm thấy Role");
                            }
                        }
                        else
                        {
                            // Nếu không có RoleId hoặc không tìm thấy user, vẫn xem như tạo user thành công
                            TempData["success"] = "User đã được tạo thành công";
                            return RedirectToAction("Index", "User");
                        }
                    }
                    else
                    {
                        // Nếu tạo user thất bại, thêm các lỗi vào ModelState
                        AddIdentityErrors(createUserResult);
                    }
                }

                // Nếu ModelState không hợp lệ hoặc có lỗi xảy ra trong quá trình xử lý
                // Lấy danh sách roles để hiển thị lại trong view
                var roles = await _roleManager.Roles.ToListAsync();
                ViewBag.Roles = new SelectList(roles, "Id", "Name");
                // Trả về view với model hiện tại để hiển thị lỗi và giữ lại dữ liệu đã nhập
                return View(user);
            }

            [HttpGet]
            [Route("Delete/{id}")]
            public async Task<IActionResult> Delete(string Id)
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return NotFound();
                }    
                var user = await _userManager.FindByIdAsync(Id);
                if (user == null)
                {
                    return NotFound();
                }
                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    return View("Errors");
                }
                TempData["success"] = "User này đã được xóa";
                return RedirectToAction("Index");
                
            }
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> LockedUsers()
            {
                var users = _userManager.Users
                            .Where(u => u.LockoutEndTime != null && u.LockoutEndTime > DateTime.Now)
                            .ToList();

                return View(users);
            }
            [HttpPost]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> UnlockUser(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.LockoutEndTime = null;
                    user.FailedLoginAttempts = 0;
                    await _userManager.UpdateAsync(user);
                }

                return RedirectToAction("Index");
            }

    }
}