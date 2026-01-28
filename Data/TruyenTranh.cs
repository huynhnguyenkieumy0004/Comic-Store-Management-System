using System;
using System.Collections.Generic;

namespace Comic.Data;

public partial class TruyenTranh
{
    public int MaTruyen { get; set; }

    public string TenTruyen { get; set; } = null!;

    public string? TacGia { get; set; }

    public string? MoTa { get; set; }

    public string? Image { get; set; }

    public DateOnly? NgayDang { get; set; }

    public int? SoLuotXem { get; set; }

    public bool? IsUnique { get; set; }

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual ICollection<Chuong> Chuongs { get; set; } = new List<Chuong>();

    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();

    public virtual ICollection<ThongKeTruyen> ThongKeTruyens { get; set; } = new List<ThongKeTruyen>();

    public virtual ICollection<ThuVien> ThuViens { get; set; } = new List<ThuVien>();

    public virtual ICollection<TheLoai> MaTheLoais { get; set; } = new List<TheLoai>();
}
