using System;
using System.Collections.Generic;

namespace Comic.Data;

public partial class BinhLuan
{
    public int MaBinhLuan { get; set; }

    public long? MaNguoiDung { get; set; }

    public string NoiDung { get; set; } = null!;

    public DateTime? NgayTao { get; set; }

    public string? LoaiDoiTuong { get; set; }

    public int? MaTruyen { get; set; }

    public int? MaChuong { get; set; }

    // Khóa ngoại trả lời bình luận khác
    public int? MaBinhLuanCha { get; set; } // null nếu là bình luận gốc

    // Navigation properties
    public virtual BinhLuan? BinhLuanCha { get; set; }
    public virtual ICollection<BinhLuan> BinhLuanCon { get; set; } = new List<BinhLuan>();

    public virtual Chuong? MaChuongNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual TruyenTranh? MaTruyenNavigation { get; set; }
}
