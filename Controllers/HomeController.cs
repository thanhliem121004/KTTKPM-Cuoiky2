using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace E_commerceTechnologyWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _dataContext = context;
        }

        public IActionResult Index(string sort_by = "")
        {
            IQueryable<SanPhamModel> products = _dataContext.SanPham.Include("TheLoai").Include("ThuongHieu");

            switch (sort_by)
            {
                case "price_increase":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_decrease":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "price_newest":
                    products = products.OrderByDescending(p => p.Id);
                    break;
                case "price_oldest":
                    products = products.OrderBy(p => p.Id);
                    break;
                default:
                    products = products.OrderByDescending(p => p.Id);
                    break;
            }

            return View(products.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            var contact = _dataContext.Contact.FirstOrDefault();
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult HelpCenter()
        {
            return View();
        }

        public IActionResult PurchaseGuide()
        {
            return View();
        }

        public IActionResult WarrantyPolicy()
        {
            return View();
        }

        public IActionResult Careers()
        {
            return View();
        }

        public IActionResult TermsOfUse()
        {
            return View();
        }
    }
}
