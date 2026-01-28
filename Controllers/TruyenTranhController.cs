using Comic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace Comic.Controllers
{
    public class TruyenTranhController : Controller
    {

        private readonly TomComicContext _context;

        public TruyenTranhController(TomComicContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var truyen = _context.TruyenTranhs
    .Include(t => t.MaTheLoais) // collection
    .ToList();

            return View(truyen);
        }

        // ===== Action lọc theo thể loại =====
        public IActionResult TheLoai(int id)
        {
            // id = 1 nghĩa là 'Tất cả'
            if (id == 1)
            {
                var allTruyen = _context.TruyenTranhs
                    .Include(t => t.MaTheLoais) // include collection
                    .ToList();

                return View("Index", allTruyen);
            }

            // Lọc truyện theo thể loại (many-to-many)
            var truyenTheoLoai = _context.TruyenTranhs
                .Include(t => t.MaTheLoais)
                .Where(t => t.MaTheLoais.Any(tl => tl.MaTheLoai == id)) // kiểm tra trong collection
                .ToList();

            return View("Index", truyenTheoLoai);
        }


        public async Task<IActionResult> Details(int MaTruyen)
        {
            if (MaTruyen == 0)
                return RedirectToAction("Index");

            var truyen = await _context.TruyenTranhs
                .Include(t => t.MaTheLoais)
                .Include(t => t.BinhLuans)
                    .ThenInclude(bl => bl.MaNguoiDungNavigation)
                .Include(t => t.DanhGia)
                    .ThenInclude(dg => dg.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(t => t.MaTruyen == MaTruyen);


            if (truyen == null)
                return NotFound();

            // ✅ Tăng số lượt xem
            truyen.SoLuotXem = (truyen.SoLuotXem ?? 0) + 1;
            _context.Update(truyen);
            await _context.SaveChangesAsync();

            var danhGia = await _context.DanhGia
    .Where(dg => dg.MaTruyen == MaTruyen)
    .Include(dg => dg.MaNguoiDungNavigation)
    .ToListAsync();

            ViewBag.DanhGiaList = danhGia;


            var chuongs = await _context.Chuongs
                .Where(c => c.MaTruyen == MaTruyen)
                .OrderBy(c => c.SoChuong)
                .ToListAsync();

            // Thống kê rating
            var ratings = truyen.DanhGia.Select(d => d.Ratings ?? 0).ToList();
            ViewBag.TotalRatings = ratings.Count;
            ViewBag.AverageRating = ratings.Count > 0 ? ratings.Average() : 0;
            ViewBag.RatingBreakdown = ratings
                .GroupBy(r => r)
                .ToDictionary(g => g.Key, g => g.Count());

            ViewBag.MaTruyen = MaTruyen;
            ViewBag.BinhLuanList = truyen.BinhLuans.OrderByDescending(bl => bl.NgayTao).ToList();
            ViewBag.ChuongList = chuongs;

            ViewBag.TruyenDeCu = await _context.TruyenTranhs
                .Where(t => t.MaTruyen != MaTruyen && t.IsUnique == true)
                .Take(6)
                .ToListAsync();

            return View(truyen);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var allTruyen = await _context.TruyenTranhs
                    .Include(t => t.MaTheLoais) // include collection
                    .ToListAsync();

                ViewBag.Keyword = "Tất cả truyện";
                return View("Index", allTruyen);
            }

            string keyword = RemoveDiacritics(searchTerm.Trim().ToLower());

            var truyenList = await _context.TruyenTranhs
                .Include(t => t.MaTheLoais) // include collection
                .ToListAsync();

            var filteredTruyen = truyenList
                .Where(t =>
                    RemoveDiacritics(t.TenTruyen.ToLower()).Contains(keyword) ||
                    RemoveDiacritics((t.TacGia ?? "").ToLower()).Contains(keyword) ||
                    (!string.IsNullOrEmpty(t.MoTa) && RemoveDiacritics(t.MoTa.ToLower()).Contains(keyword))
                )
                .ToList();

            ViewBag.Keyword = $"Kết quả cho '{searchTerm}'";
            return View("Index", filteredTruyen);
        }


        // Hàm bỏ dấu tiếng Việt
        private string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }


    }
}
