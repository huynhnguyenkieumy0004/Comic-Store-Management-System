using System.Diagnostics;
using Comic.Data;
using Comic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comic.Controllers
{
    public class HomeController : Controller
    {
        private readonly TomComicContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, TomComicContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // L?y t?t c? truy?n kèm th? lo?i
            var truyen = _context.TruyenTranhs
                .Include(sp => sp.MaTheLoais) // collection many-to-many
                .ToList();

            // L?y truy?n ?? c?
            ViewBag.TruyenDeCu = _context.TruyenTranhs
                .Include(t => t.MaTheLoais) // include collection
                .Where(t => t.IsUnique == true)
                .Take(6) // l?y t?i ?a 6 truy?n
                .ToList();

            return View(truyen);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Intro()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
