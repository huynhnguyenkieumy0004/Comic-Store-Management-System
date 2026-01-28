using Comic.Data;
using Comic.Models; // nếu BinhLuan model nằm đây
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comic.Controllers
{
    public class BinhLuanController : Controller
    {
        private readonly TomComicContext _context;

        public BinhLuanController(TomComicContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách bình luận theo truyện
        public async Task<IActionResult> Index(int maTruyen)
        {
            var binhLuan = await _context.BinhLuans
        .Where(bl => bl.MaTruyen == maTruyen && bl.MaBinhLuanCha == null) // chỉ lấy bình luận gốc
        .Include(bl => bl.BinhLuanCon)
            .ThenInclude(r => r.MaNguoiDungNavigation) // load user cho reply
        .Include(bl => bl.MaNguoiDungNavigation)      // load user cho bình luận gốc
        .OrderByDescending(bl => bl.NgayTao)
        .ToListAsync();

            ViewBag.MaTruyen = maTruyen;
            return View(binhLuan);
        }

        // Thêm bình luận
        [HttpPost]
        public async Task<IActionResult> Create(int maTruyen, string noiDung, int? maBinhLuanCha)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "NguoiDung");
            }

            var bl = new BinhLuan
            {
                MaNguoiDung = userId.Value,
                NoiDung = noiDung,
                NgayTao = DateTime.Now,
                MaTruyen = maTruyen,
                MaBinhLuanCha = maBinhLuanCha
            };

            _context.BinhLuans.Add(bl);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "TruyenTranh", new { MaTruyen = maTruyen });
        }


        // Xóa bình luận
        [HttpPost]
        public async Task<IActionResult> Delete(int maBinhLuan, int maTruyen)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
                return RedirectToAction("Login", "NguoiDung");

            var bl = await _context.BinhLuans
                .Include(b => b.BinhLuanCon) // cần navigation property Children
                .FirstOrDefaultAsync(b => b.MaBinhLuan == maBinhLuan);

            if (bl != null && bl.MaNguoiDung == currentUserId.Value)
            {
                DeleteWithChildren(bl);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "TruyenTranh", new { MaTruyen = maTruyen });
        }

        // Xóa đệ quy
        private void DeleteWithChildren(BinhLuan bl)
        {
            if (bl.BinhLuanCon != null)
            {
                foreach (var child in bl.BinhLuanCon.ToList())
                {
                    DeleteWithChildren(child);
                }
            }
            _context.BinhLuans.Remove(bl);
        }



    }
}
