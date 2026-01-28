using Comic.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comic.Controllers
{
    public class NguoiDungController : Controller
    {

        private readonly TomComicContext _context;

        public NguoiDungController(TomComicContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // ✅ Hiển thị trang đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Clear(); // reset thông tin user cũ
            return View();
        }

        // ✅ Xử lý đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(string tenDangNhap, string matKhau)
        {

            var user = await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap && u.MatKhau == matKhau);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", (int)user.MaNguoiDung);
                HttpContext.Session.SetString("TenNguoiDung", user.TenNguoiDung);
                HttpContext.Session.SetInt32("IsAdmin", user.Admin); // 1 = admin, 0 = user

                if (user.Admin == 1)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" }); // 🔥 Vào Admin
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "" }); // 🔥 Vào User
                }
                
            }

            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu sai!";
            return View();
        }

        // ✅ Hiển thị trang đăng ký
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ✅ Xử lý đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên đăng nhập đã tồn tại chưa
                var exists = await _context.NguoiDungs.AnyAsync(u => u.TenDangNhap == model.TenDangNhap);
                if (exists)
                {
                    ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại!");
                    return View(model);
                }

                model.NgayDangKy = DateTime.Now;
                model.Admin = 0; // Mặc định không phải Admin

                _context.Add(model);
                await _context.SaveChangesAsync();

                // Sau khi đăng ký, chuyển hướng đến Login
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa toàn bộ session
                                         // Nếu dùng cookie:
                                         // Response.Cookies.Delete("UserCookie");
            return RedirectToAction("Login", "NguoiDung");
        }

        // Profile
        // Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            long? uid = HttpContext.Session.GetInt32("UserId"); // ✅ Int64
            if (uid == null) return RedirectToAction("Login");

            var user = await _context.NguoiDungs.FindAsync(uid.Value);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(NguoiDung model)
        {
            long? uid = HttpContext.Session.GetInt32("UserId"); // ✅ Int64
            if (uid == null) return RedirectToAction("Login");

            var user = await _context.NguoiDungs.FindAsync(uid.Value);
            if (user == null) return NotFound();

            user.TenNguoiDung = model.TenNguoiDung;
            user.Email = model.Email;

            await _context.SaveChangesAsync(); // ✅ không cần Update
            ViewBag.Success = "Cập nhật thành công!";

            return View("Profile", user);
        }



        // Change Password
        [HttpGet]
        public IActionResult ChangePassword()
        {
            long? uid = HttpContext.Session.GetInt32("UserId"); // ✅ Int64
            if (uid == null) return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            long? uid = HttpContext.Session.GetInt32("UserId"); // ✅ Int64
            if (uid == null) return RedirectToAction("Login");

            var user = await _context.NguoiDungs.FindAsync(uid.Value);
            if (user == null) return NotFound();

            if (user.MatKhau != oldPassword)
            {
                ViewBag.Error = "Mật khẩu cũ không đúng!";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu mới và xác nhận không khớp!";
                return View();
            }

            user.MatKhau = newPassword;
            await _context.SaveChangesAsync();

            ViewBag.Success = "Đổi mật khẩu thành công!";
            return View();
        }


    }
}
