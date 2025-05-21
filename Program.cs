using AspNetCoreRateLimit;
using E_commerceTechnologyWebsite.Areas.Admin.KhoLuuTru;
using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Middleware;
using E_commerceTechnologyWebsite.Models;
using E_commerceTechnologyWebsite.Models.Momo;
using E_commerceTechnologyWebsite.Services.Momo;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using reCAPTCHA.AspNetCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Kết nối Momo
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();

// Kết nối Database
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectDb"]);
}, ServiceLifetime.Scoped);

// Gửi email
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Cấu hình View + Session
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//reCAPCHA
builder.Services.AddRecaptcha(options =>
{
    options.SiteKey = "6LdGyzArAAAAAKNBO08N9dYDYV0Nwm49nUrnwYyQ";
    options.SecretKey = "6LdGyzArAAAAALL4OhZhkY5cQIMQ1l7mk8-IIFqH";
});

// Cấu hình Identity (phải trước Authentication)
builder.Services.AddIdentity<AppUserModel, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Cấu hình mật khẩu
    options.Password.RequireDigit = true; // Yêu cầu có số
    options.Password.RequireLowercase = true; // Yêu cầu có chữ thường
    options.Password.RequireUppercase = true; // Yêu cầu có chữ hoa
    options.Password.RequireNonAlphanumeric = true; // Yêu cầu có ký tự đặc biệt
    options.Password.RequiredLength = 8; // Yêu cầu độ dài tối thiểu là 8 ký tự

    // Cấu hình tài khoản
    options.User.RequireUniqueEmail = true; // Yêu cầu email là duy nhất
});
// gg

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
    IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    googleOptions.ClientId = googleAuthNSection["ClientId"];
    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
});

// Logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

// Thêm các services Rate Limiting
// Add services to the container.
builder.Services.AddControllersWithViews();

// Rate limiting services
builder.Services.AddMemoryCache();
// Configure rate limiting
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddInMemoryRateLimiting();


// Khởi tạo app
var app = builder.Build();

// Middleware pipeline
app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); // Trước Authentication
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

// Use rate limiting
app.UseIpRateLimiting();


// Route mặc định
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=SanPham}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "TheLoai",
    pattern: "/theloai/{Slug?}",
    defaults: new { controller = "TheLoai", action = "Index" });

app.MapControllerRoute(
    name: "ThuongHieu",
    pattern: "/thuonghieu/{Slug?}",
    defaults: new { controller = "ThuongHieu", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SanPham}/{action=Index}/{id?}");

// Seed dữ liệu
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    SeedData.SeedingData(context);
}

//rate limiting
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        if (context.Response.Headers.TryGetValue("X-RateLimit-Remaining", out var remaining))
        {
            Console.WriteLine($"Rate limit remaining: {remaining} for IP: {context.Connection.RemoteIpAddress}");
        }
        return Task.CompletedTask;
    });

    await next();
});



// Thêm security headers 
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";

    if (context.Request.IsHttps)
    {
        context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
    }

    await next();
});

// Thêm Request Logging Middleware
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var sw = Stopwatch.StartNew();

    try
    {
        await next(context);
    }
    finally
    {
        sw.Stop();
        logger.LogInformation(
            "Request {method} {url} => {statusCode} ({elapsed}ms)",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            sw.ElapsedMilliseconds);
    }
});

//Theo dõi hoạt động User Session Tracking
app.UseMiddleware<SessionTrackingMiddleware>();

app.Run();

