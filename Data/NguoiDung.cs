using System;
using System.Collections.Generic;

namespace Comic.Data;

public partial class NguoiDung
{
    public long MaNguoiDung { get; set; }

    public string TenNguoiDung { get; set; } = null!;

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Admin { get; set; }

    public DateTime? NgayDangKy { get; set; }

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();

    public virtual ICollection<ThuVien> ThuViens { get; set; } = new List<ThuVien>();
}
