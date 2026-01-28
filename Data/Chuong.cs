using System;
using System.Collections.Generic;

namespace Comic.Data;

public partial class Chuong
{
    public int MaChuong { get; set; }

    public int? MaTruyen { get; set; }

    public int SoChuong { get; set; }

    public string? TieuDe { get; set; }

    public DateOnly? NgayDang { get; set; }

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual TruyenTranh? MaTruyenNavigation { get; set; }

    public virtual ICollection<TrangAnh> TrangAnhs { get; set; } = new List<TrangAnh>();
}
