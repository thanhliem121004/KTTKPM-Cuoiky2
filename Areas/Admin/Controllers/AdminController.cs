using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceTechnologyWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly DataContext _context;

        public AdminController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> UserActivities()
        {
            var activities = await _context.UserActivities
                .OrderByDescending(a => a.Timestamp)
                .Take(100)  // Lấy 100 hoạt động gần nhất
                .ToListAsync();

            return View(activities);
        }
    }
}