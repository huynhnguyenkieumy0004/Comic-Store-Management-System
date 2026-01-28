using System;
using System.Collections.Generic;

namespace Comic.Data;

public partial class ThongKeTruyen
{
    public int MaThongKe { get; set; }

    public int? MaTruyen { get; set; }

    public int? SoLuotXem { get; set; }

    public int? SoLuotXemTuan { get; set; }

    public double? DiemTrungBinh { get; set; }

    public bool? LaDocDao { get; set; }

    public virtual TruyenTranh? MaTruyenNavigation { get; set; }
}
