using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerceTechnologyWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("test-rate-limit")]
        public IActionResult TestRateLimit()
        {
            var remaining = HttpContext.Response.Headers["X-RateLimit-Remaining"].FirstOrDefault();
            return Ok($"Success. Remaining: {remaining}");
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var lastActivityUtc = HttpContext.Session.GetString("LastActivity");
            var currentTimeUtc = DateTime.UtcNow;

            // Chuyển đổi UTC sang giờ Việt Nam (UTC+7)
            var vietnamOffset = TimeSpan.FromHours(7);
            var currentTimeVietnam = currentTimeUtc.Add(vietnamOffset);
            var lastActivityVietnam = lastActivityUtc != null
                ? DateTime.Parse(lastActivityUtc).Add(vietnamOffset)
                : (DateTime?)null;

            var viewModel = new
            {
                UserId = userId,
                LastActivity = lastActivityVietnam?.ToString("yyyy-MM-dd HH:mm:ss"),
                CurrentTime = currentTimeVietnam.ToString("yyyy-MM-dd HH:mm:ss"),
                TimeRemaining = lastActivityUtc != null
                    ? Math.Max(0, 30 - (currentTimeUtc - DateTime.Parse(lastActivityUtc)).TotalMinutes)
                    : 0
            };

            return Ok(viewModel);
        }
    }



}
