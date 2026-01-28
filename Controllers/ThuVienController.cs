using Comic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comic.Controllers
{
    public class ThuVienController : Controller
    {
        private readonly TomComicContext _context;

        public ThuVienController(TomComicContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "NguoiDung");
            }

            var thuVien = await _context.ThuViens
                .Include(tv => tv.MaTruyenNavigation)
                    .ThenInclude(t => t.MaTheLoais)
                .Where(tv => tv.MaNguoiDung == userId)
                .OrderByDescending(tv => tv.NgayLuu)
                .ToListAsync();

            return View(thuVien);
        }

        [HttpPost]
        public async Task<IActionResult> AddToLibrary(int maTruyen)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(new { success = false, message = "Bạn chưa đăng nhập." });
            }

            var exists = await _context.ThuViens
                .AnyAsync(tv => tv.MaNguoiDung == userId && tv.MaTruyen == maTruyen);

            if (exists)
            {
                return Json(new { success = false, message = "Truyện đã có trong thư viện." });
            }

            var thuVien = new ThuVien
            {
                MaNguoiDung = userId.Value,
                MaTruyen = maTruyen,
                NgayLuu = DateTime.Now
            };

            _context.ThuViens.Add(thuVien);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã thêm vào thư viện." });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromLibrary(int maTruyen)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(new { success = false, message = "Bạn chưa đăng nhập." });
            }

            var item = await _context.ThuViens
                .FirstOrDefaultAsync(tv => tv.MaNguoiDung == userId && tv.MaTruyen == maTruyen);

            if (item == null)
            {
                return Json(new { success = false, message = "Truyện không tồn tại trong thư viện." });
            }

            _context.ThuViens.Remove(item);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã xóa khỏi thư viện." });
        }
    }
}
