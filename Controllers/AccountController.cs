using E_commerceTechnologyWebsite.Models;
using E_commerceTechnologyWebsite.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using E_commerceTechnologyWebsite.Areas.Admin.KhoLuuTru;
using System.Text.Encodings.Web;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace E_commerceTechnologyWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly SignInManager<AppUserModel> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<AppUserModel> userManager,
                                 SignInManager<AppUserModel> signInManager,
                                 IEmailSender emailSender,
                                 IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginVM.Username);
                if (user != null)
                {
                    // Kiểm tra nếu tài khoản đang bị khóa
                    if (user.LockoutEndTime.HasValue && user.LockoutEndTime > DateTime.Now)
                    {
                        ModelState.AddModelError("", "Tài khoản của bạn đang bị khóa. Vui lòng thử lại sau.");
                        return View(loginVM);
                    }

                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        // Reset số lần đăng nhập sai nếu đăng nhập thành công
                        user.FailedLoginAttempts = 0;
                        user.LockoutEndTime = null;
                        await _userManager.UpdateAsync(user);

                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            return Redirect("/admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "SanPham");
                        }
                    }
                    else
                    {
                        // Tăng số lần đăng nhập sai
                        user.FailedLoginAttempts += 1;

                        // Nếu vượt quá 5 lần, khóa tài khoản 15 phút
                        if (user.FailedLoginAttempts >= 5)
                        {
                            user.LockoutEndTime = DateTime.Now.AddMinutes(15);
                            ModelState.AddModelError("", "Tài khoản đã bị khóa do nhập sai quá nhiều lần. Thử lại sau 15 phút.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                        }

                        await _userManager.UpdateAsync(user);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                }
            }

            return View(loginVM);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                // Xác thực reCAPTCHA
                var captchaResponse = Request.Form["g-recaptcha-response"];
                var secretKey = _configuration["RecaptchaSettings:SecretKey"];
                var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";

                var client = new WebClient();
                var result_Captcha = client.DownloadString(string.Format(apiUrl, secretKey, captchaResponse));
                var obj = JObject.Parse(result_Captcha);
                var status = (bool)obj.SelectToken("success");

                if (!status)
                {
                    ModelState.AddModelError("", "Vui lòng xác nhận reCAPTCHA.");
                    return View(user);
                }

                // Tiếp tục xử lý đăng ký nếu reCAPTCHA hợp lệ
                AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email };
                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "Tạo tài khoản thành công";
                    return RedirectToAction("Login", "Account");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(returnUrl);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                // Add other user properties as needed
            };

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                // Add other editable properties
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false });
            }

            user.PhoneNumber = model.PhoneNumber;
            // Update other properties as needed

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tìm người dùng qua email
                var user = await _userManager.FindByEmailAsync(model.Email);

                // Nếu người dùng không tồn tại hoặc email chưa xác nhận
                if (user == null /*|| !(await _userManager.IsEmailConfirmedAsync(user))*/)
                {
                    // Không tiết lộ rằng người dùng không tồn tại hoặc chưa xác nhận
                    return RedirectToAction(nameof(ForgotPasswordNullInfo));
                }

                // Tạo mã reset mật khẩu
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Tạo đường dẫn callback để reset mật khẩu
                var callbackUrl = Url.Action(
                    "ResetPassword", // Hành động reset password
                    "Account", // Controller Account
                    new { userId = user.Id, code = code }, // Tham số userId và code
                    protocol: HttpContext.Request.Scheme); // Đảm bảo sử dụng đúng scheme (http hoặc https)

                // Tạo nội dung email
                var emailBody = $"Vui lòng đặt lại mật khẩu bằng cách <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>nhấp vào đây</a>.";

                // Gửi email
                await _emailSender.SendEmailAsync(model.Email, "Đặt lại mật khẩu", emailBody);

                // Chuyển hướng đến trang xác nhận quên mật khẩu
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // Nếu model không hợp lệ, trả về lại form quên mật khẩu với thông báo lỗi
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPasswordNullInfo()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return BadRequest("Cần có mã để đặt lại mật khẩu.");
            }
            else
            {
                var model = new ResetPasswordViewModel { Code = code };
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Không tiết lộ rằng người dùng không tồn tại
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            try
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    TempData["error"] = "Không thể lấy thông tin đăng nhập từ Google.";
                    return RedirectToAction("Login");
                }

                // Đăng nhập nếu đã tồn tại
                var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
                if (signInResult.Succeeded)
                {
                    TempData["success"] = "Đăng nhập thành công bằng Google.";
                    return RedirectToAction("Index", "Home");
                }

                // Lấy thông tin từ Google
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);

                if (string.IsNullOrEmpty(email))
                {
                    TempData["error"] = "Không thể lấy email từ tài khoản Google.";
                    return RedirectToAction("Login");
                }

                // Kiểm tra người dùng trong hệ thống
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new AppUserModel
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        TempData["error"] = "Không thể tạo tài khoản mới.";
                        return RedirectToAction("Login");
                    }
                }

                // Liên kết tài khoản Google nếu chưa có
                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (!addLoginResult.Succeeded)
                {
                    TempData["error"] = "Không thể liên kết thông tin đăng nhập Google.";
                    return RedirectToAction("Login");
                }

                // Đăng nhập người dùng
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["success"] = "Đăng nhập thành công bằng Google.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Đã xảy ra lỗi khi xử lý đăng nhập Google: " + ex.Message;
                return RedirectToAction("Login");
            }
        }
    }

}
