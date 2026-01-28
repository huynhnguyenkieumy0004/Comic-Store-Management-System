using System;
using System.Collections.Generic;

namespace Comic.Data;

public partial class ThuVien
{
    public int MaThuVien { get; set; }

    public long? MaNguoiDung { get; set; }

    public int? MaTruyen { get; set; }

    public DateTime? NgayLuu { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual TruyenTranh? MaTruyenNavigation { get; set; }
}
