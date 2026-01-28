using Comic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comic.Controllers
{
    public class ChuongController : Controller
    {
        private readonly TomComicContext _context;

        public ChuongController(TomComicContext context)
        {
            _context = context;
        }

        // GET: Chuong/Details/5
        public async Task<IActionResult> Details(int id) // id = MaChuong
        {
            if (id == 0)
                return RedirectToAction("Index", "TruyenTranh");

            // Lấy thông tin chương
            var chuong = await _context.Chuongs
                .Include(c => c.TrangAnhs.OrderBy(t => t.SoTrang)) // các trang ảnh theo thứ tự
                .Include(c => c.MaTruyenNavigation) // thông tin truyện
                .FirstOrDefaultAsync(c => c.MaChuong == id);

            if (chuong == null)
                return NotFound();

            // Lấy danh sách tất cả chương của truyện
            var chuongList = await _context.Chuongs
                .Where(c => c.MaTruyen == chuong.MaTruyen)
                .OrderBy(c => c.SoChuong)
                .ToListAsync();

            // Tìm chương trước và chương sau
            var index = chuongList.FindIndex(c => c.MaChuong == id);
            ViewBag.PreviousChapterId = index > 0 ? chuongList[index - 1].MaChuong : (int?)null;
            ViewBag.NextChapterId = index < chuongList.Count - 1 ? chuongList[index + 1].MaChuong : (int?)null;

            ViewBag.ChuongList = chuongList; // danh sách chương cho sidebar
            ViewBag.CurrentChapterId = id; // chương hiện tại

            return View(chuong);
        }
    }
}
