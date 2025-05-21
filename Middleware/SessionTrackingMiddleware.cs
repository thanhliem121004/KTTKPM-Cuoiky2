using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_commerceTechnologyWebsite.Middleware
{
    public class SessionTrackingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public SessionTrackingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<SessionTrackingMiddleware>();
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation($"User {userId} accessed {context.Request.Path} at {DateTime.UtcNow}");

                // Lưu thông tin hoạt động vào database
                await SaveUserActivityAsync(userId, context);

                // Cập nhật thời gian hoạt động cuối cùng
                context.Session.SetString("LastActivity", DateTime.UtcNow.ToString());

                // Kiểm tra thời gian không hoạt động
                var lastActivityString = context.Session.GetString("LastActivity");
                if (DateTime.TryParse(lastActivityString, out DateTime lastActivity))
                {
                    if (DateTime.UtcNow - lastActivity > TimeSpan.FromMinutes(30))
                    {
                        _logger.LogWarning($"User {userId} session expired due to inactivity");
                        await context.SignOutAsync();
                        context.Response.Redirect("/Account/Login");
                        return;
                    }
                }
            }

            await _next(context);
        }

        private async Task SaveUserActivityAsync(string userId, HttpContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var userActivity = new UserActivity
                {
                    UserId = userId,
                    Path = context.Request.Path,
                    Timestamp = DateTime.UtcNow,
                    IpAddress = context.Connection.RemoteIpAddress?.ToString()
                };

                dbContext.UserActivities.Add(userActivity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}