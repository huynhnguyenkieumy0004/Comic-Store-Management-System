using Comic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Comic.Controllers
{
    public class DanhGiaController : Controller
    {
        private readonly TomComicContext _context;

        public DanhGiaController(TomComicContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách đánh giá
        public async Task<IActionResult> Index(int maTruyen)
        {
            var danhGia = await _context.DanhGia
    .Where(dg => dg.MaTruyen == maTruyen)
    .Include(dg => dg.MaNguoiDungNavigation)
    .ToListAsync();


            ViewBag.MaTruyen = maTruyen;
            ViewBag.DanhGiaList = danhGia;  // 👈 thêm dòng này

            if (danhGia.Any())
            {
                ViewBag.Total = danhGia.Count;
                ViewBag.Average = danhGia.Average(d => d.Ratings);
                ViewBag.Count1 = danhGia.Count(d => d.Ratings == 1);
                ViewBag.Count2 = danhGia.Count(d => d.Ratings == 2);
                ViewBag.Count3 = danhGia.Count(d => d.Ratings == 3);
                ViewBag.Count4 = danhGia.Count(d => d.Ratings == 4);
                ViewBag.Count5 = danhGia.Count(d => d.Ratings == 5);
            }

            return View(danhGia);
        }


        // Thêm hoặc cập nhật đánh giá
        [HttpPost]
        public async Task<IActionResult> Rate(int maTruyen, int ratings)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "NguoiDung");
            if (ratings < 1 || ratings > 5) return BadRequest();

            var existing = await _context.DanhGia
                .FirstOrDefaultAsync(dg => dg.MaTruyen == maTruyen && dg.MaNguoiDung == userId.Value);

            if (existing != null)
            {
                existing.Ratings = ratings;
                existing.NgayDanhGia = DateTime.Now;
                _context.DanhGia.Update(existing);
            }
            else
            {
                var dg = new DanhGia
                {
                    MaTruyen = maTruyen,
                    MaNguoiDung = userId.Value,
                    Ratings = ratings,
                    NgayDanhGia = DateTime.Now
                };
                _context.DanhGia.Add(dg);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "TruyenTranh", new { MaTruyen = maTruyen });
        }

        // GET: Edit đánh giá
        [HttpGet]
        public async Task<IActionResult> Edit(int maDanhGia)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "NguoiDung");

            var dg = await _context.DanhGia
                .FirstOrDefaultAsync(d => d.MaDanhGia == maDanhGia && d.MaNguoiDung == userId.Value);

            if (dg == null) return NotFound();

            // Tạo SelectList 1-5 để view dùng asp-items
            ViewBag.RatingList = new SelectList(Enumerable.Range(1, 5), dg.Ratings);

            return View(dg);
        }

        // POST: Edit đánh giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int maDanhGia, int ratings)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "NguoiDung");
            if (ratings < 1 || ratings > 5) return BadRequest();

            var dg = await _context.DanhGia
                .FirstOrDefaultAsync(d => d.MaDanhGia == maDanhGia && d.MaNguoiDung == userId.Value);

            if (dg == null) return NotFound();

            dg.Ratings = ratings;
            dg.NgayDanhGia = DateTime.Now;
            _context.DanhGia.Update(dg);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "TruyenTranh", new { MaTruyen = dg.MaTruyen });
        }

        // GET: Thống kê đánh giá
        public async Task<IActionResult> Stats(int maTruyen)
        {
            var danhGia = await _context.DanhGia
                .Where(dg => dg.MaTruyen == maTruyen)
                .ToListAsync();

            if (!danhGia.Any())
            {
                ViewBag.Message = "Truyện chưa có đánh giá nào.";
                return View();
            }

            var stats = new
            {
                Total = danhGia.Count,
                Average = danhGia.Average(d => d.Ratings),
                Count1 = danhGia.Count(d => d.Ratings == 1),
                Count2 = danhGia.Count(d => d.Ratings == 2),
                Count3 = danhGia.Count(d => d.Ratings == 3),
                Count4 = danhGia.Count(d => d.Ratings == 4),
                Count5 = danhGia.Count(d => d.Ratings == 5)
            };

            ViewBag.Stats = stats;
            ViewBag.MaTruyen = maTruyen;

            return View();
        }

    }
}
