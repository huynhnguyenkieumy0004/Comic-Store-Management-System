using System;
using System.Collections.Generic;

namespace Comic.Data;

public partial class TheLoai
{
    public int MaTheLoai { get; set; }

    public string TenTheLoai { get; set; } = null!;

    public virtual ICollection<TruyenTranh> MaTruyens { get; set; } = new List<TruyenTranh>();
}
