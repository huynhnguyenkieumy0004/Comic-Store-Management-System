using System;
using System.Collections.Generic;

namespace Comic.Data;

public partial class TrangAnh
{
    public int MaTrang { get; set; }

    public int? MaChuong { get; set; }

    public int SoTrang { get; set; }

    public string DuongDan { get; set; } = null!;

    public virtual Chuong? MaChuongNavigation { get; set; }
}
