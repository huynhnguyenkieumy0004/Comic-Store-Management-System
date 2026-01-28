using System;
using System.Collections.Generic;

namespace Comic.Data;

public partial class DanhGia
{
    public int MaDanhGia { get; set; }

    public int? MaTruyen { get; set; }

    public long? MaNguoiDung { get; set; }

    public int? Ratings { get; set; }

    public DateTime? NgayDanhGia { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual TruyenTranh? MaTruyenNavigation { get; set; }
}
