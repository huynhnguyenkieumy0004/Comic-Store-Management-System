using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Comic.Data;

public partial class TomComicContext : DbContext
{
    public TomComicContext()
    {
    }

    public TomComicContext(DbContextOptions<TomComicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BinhLuan> BinhLuans { get; set; }

    public virtual DbSet<Chuong> Chuongs { get; set; }

    public virtual DbSet<DanhGia> DanhGia { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<TheLoai> TheLoais { get; set; }

    public virtual DbSet<ThongKeTruyen> ThongKeTruyens { get; set; }

    public virtual DbSet<ThuVien> ThuViens { get; set; }

    public virtual DbSet<TrangAnh> TrangAnhs { get; set; }

    public virtual DbSet<TruyenTranh> TruyenTranhs { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-E27QIPK\\SQLEXPRESS;Initial Catalog=TomComic;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.HasKey(e => e.MaBinhLuan).HasName("PK__BinhLuan__300DD2D8B7B8C13C");

            entity.ToTable("BinhLuan");

            entity.Property(e => e.MaBinhLuan).HasColumnName("ma_binh_luan");
            entity.Property(e => e.LoaiDoiTuong)
                .HasMaxLength(10)
                .HasColumnName("loai_doi_tuong");
            entity.Property(e => e.MaChuong).HasColumnName("ma_chuong");
            entity.Property(e => e.MaNguoiDung).HasColumnName("ma_nguoi_dung");
            entity.Property(e => e.MaTruyen).HasColumnName("ma_truyen");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.NoiDung).HasColumnName("noi_dung");

            entity.HasOne(d => d.MaChuongNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaChuong)
                .HasConstraintName("FK__BinhLuan__ma_chu__5165187F");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__BinhLuan__ma_ngu__4D94879B");

            entity.HasOne(d => d.MaTruyenNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaTruyen)
                .HasConstraintName("FK__BinhLuan__ma_tru__5070F446");

            entity.Property(e => e.MaBinhLuanCha).HasColumnName("ma_binh_luan_cha");

            entity.HasOne(d => d.BinhLuanCha)
                  .WithMany(p => p.BinhLuanCon)
                  .HasForeignKey(d => d.MaBinhLuanCha)
                  .HasConstraintName("FK_BinhLuan_Cha")
                  .OnDelete(DeleteBehavior.Restrict); // tránh xóa cascade

        });

        modelBuilder.Entity<Chuong>(entity =>
        {
            entity.HasKey(e => e.MaChuong).HasName("PK__Chuong__432A749498D679D9");

            entity.ToTable("Chuong");

            entity.Property(e => e.MaChuong).HasColumnName("ma_chuong");
            entity.Property(e => e.MaTruyen).HasColumnName("ma_truyen");
            entity.Property(e => e.NgayDang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("ngay_dang");
            entity.Property(e => e.SoChuong).HasColumnName("so_chuong");
            entity.Property(e => e.TieuDe)
                .HasMaxLength(200)
                .HasColumnName("tieu_de");

            entity.HasOne(d => d.MaTruyenNavigation).WithMany(p => p.Chuongs)
                .HasForeignKey(d => d.MaTruyen)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Chuong__ma_truye__46E78A0C");
        });

        modelBuilder.Entity<DanhGia>(entity =>
        {
            entity.HasKey(e => e.MaDanhGia).HasName("PK__DanhGia__75DAD65547BD9E8B");

            entity.Property(e => e.MaDanhGia).HasColumnName("ma_danh_gia");
            entity.Property(e => e.MaNguoiDung).HasColumnName("ma_nguoi_dung");
            entity.Property(e => e.MaTruyen).HasColumnName("ma_truyen");
            entity.Property(e => e.NgayDanhGia)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_danh_gia");
            entity.Property(e => e.Ratings).HasColumnName("ratings");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__DanhGia__ma_nguo__5535A963");

            entity.HasOne(d => d.MaTruyenNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaTruyen)
                .HasConstraintName("FK__DanhGia__ma_truy__5441852A");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__19C32CF735F722E7");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.Email, "UQ__NguoiDun__AB6E6164A14A9418").IsUnique();

            entity.Property(e => e.MaNguoiDung).HasColumnName("ma_nguoi_dung");
            entity.Property(e => e.Admin).HasColumnName("admin");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .HasColumnName("mat_khau");
            entity.Property(e => e.NgayDangKy)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_dang_ky");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ten_dang_nhap");
            entity.Property(e => e.TenNguoiDung)
                .HasMaxLength(100)
                .HasColumnName("ten_nguoi_dung");
        });

        modelBuilder.Entity<TheLoai>(entity =>
        {
            entity.HasKey(e => e.MaTheLoai).HasName("PK__TheLoai__489AA0F3E0BB15BE");

            entity.ToTable("TheLoai");

            entity.Property(e => e.MaTheLoai).HasColumnName("ma_the_loai");
            entity.Property(e => e.TenTheLoai)
                .HasMaxLength(100)
                .HasColumnName("ten_the_loai");
        });

        modelBuilder.Entity<ThongKeTruyen>(entity =>
        {
            entity.HasKey(e => e.MaThongKe).HasName("PK__ThongKeT__E74586490540E523");

            entity.ToTable("ThongKeTruyen");

            entity.Property(e => e.MaThongKe).HasColumnName("ma_thong_ke");
            entity.Property(e => e.DiemTrungBinh)
                .HasDefaultValue(0.0)
                .HasColumnName("diem_trung_binh");
            entity.Property(e => e.LaDocDao)
                .HasDefaultValue(false)
                .HasColumnName("la_doc_dao");
            entity.Property(e => e.MaTruyen).HasColumnName("ma_truyen");
            entity.Property(e => e.SoLuotXem)
                .HasDefaultValue(0)
                .HasColumnName("so_luot_xem");
            entity.Property(e => e.SoLuotXemTuan)
                .HasDefaultValue(0)
                .HasColumnName("so_luot_xem_tuan");

            entity.HasOne(d => d.MaTruyenNavigation).WithMany(p => p.ThongKeTruyens)
                .HasForeignKey(d => d.MaTruyen)
                .HasConstraintName("FK__ThongKeTr__ma_tr__59FA5E80");
        });

        modelBuilder.Entity<ThuVien>(entity =>
        {
            entity.HasKey(e => e.MaThuVien).HasName("PK__ThuVien__46AE3C426837098C");

            entity.ToTable("ThuVien");

            entity.HasIndex(e => new { e.MaNguoiDung, e.MaTruyen }, "UQ__ThuVien__63B891E8AC5D6DDA").IsUnique();

            entity.Property(e => e.MaThuVien).HasColumnName("ma_thu_vien");
            entity.Property(e => e.MaNguoiDung).HasColumnName("ma_nguoi_dung");
            entity.Property(e => e.MaTruyen).HasColumnName("ma_truyen");
            entity.Property(e => e.NgayLuu)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_luu");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.ThuViens)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__ThuVien__ma_nguo__619B8048");

            entity.HasOne(d => d.MaTruyenNavigation).WithMany(p => p.ThuViens)
                .HasForeignKey(d => d.MaTruyen)
                .HasConstraintName("FK__ThuVien__ma_truy__628FA481");
        });

        modelBuilder.Entity<TrangAnh>(entity =>
        {
            entity.HasKey(e => e.MaTrang).HasName("PK__TrangAnh__633AC9ED218D966C");

            entity.ToTable("TrangAnh");

            entity.Property(e => e.MaTrang).HasColumnName("ma_trang");
            entity.Property(e => e.DuongDan)
                .HasMaxLength(255)
                .HasColumnName("duong_dan");
            entity.Property(e => e.MaChuong).HasColumnName("ma_chuong");
            entity.Property(e => e.SoTrang).HasColumnName("so_trang");

            entity.HasOne(d => d.MaChuongNavigation).WithMany(p => p.TrangAnhs)
                .HasForeignKey(d => d.MaChuong)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TrangAnh__ma_chu__4AB81AF0");
        });

        modelBuilder.Entity<TruyenTranh>(entity =>
        {
            entity.HasKey(e => e.MaTruyen).HasName("PK__TruyenTr__A7BBD1E615446E82");

            entity.ToTable("TruyenTranh");

            entity.Property(e => e.MaTruyen).HasColumnName("ma_truyen");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.IsUnique).HasDefaultValue(false);
            entity.Property(e => e.MoTa).HasColumnName("mo_ta");
            entity.Property(e => e.NgayDang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("ngay_dang");
            entity.Property(e => e.SoLuotXem)
                .HasDefaultValue(0)
                .HasColumnName("so_luot_xem");
            entity.Property(e => e.TacGia)
                .HasMaxLength(100)
                .HasColumnName("tac_gia");
            entity.Property(e => e.TenTruyen)
                .HasMaxLength(200)
                .HasColumnName("ten_truyen");

            entity.HasMany(d => d.MaTheLoais).WithMany(p => p.MaTruyens)
                .UsingEntity<Dictionary<string, object>>(
                    "TruyenTheLoai",
                    r => r.HasOne<TheLoai>().WithMany()
                        .HasForeignKey("MaTheLoai")
                        .HasConstraintName("FK__Truyen_Th__ma_th__440B1D61"),
                    l => l.HasOne<TruyenTranh>().WithMany()
                        .HasForeignKey("MaTruyen")
                        .HasConstraintName("FK__Truyen_Th__ma_tr__4316F928"),
                    j =>
                    {
                        j.HasKey("MaTruyen", "MaTheLoai").HasName("PK__Truyen_T__83327BE9297AFC34");
                        j.ToTable("Truyen_TheLoai");
                        j.IndexerProperty<int>("MaTruyen").HasColumnName("ma_truyen");
                        j.IndexerProperty<int>("MaTheLoai").HasColumnName("ma_the_loai");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
