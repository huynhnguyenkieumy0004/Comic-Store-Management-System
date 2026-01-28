-- ===============================
-- Tạo CSDL
-- ===============================
CREATE DATABASE TomComic;
GO
USE TomComic;
GO

-- ===============================
-- Bảng Người Dùng
-- ===============================
CREATE TABLE NguoiDung (
    ma_nguoi_dung BIGINT IDENTITY(1,1) PRIMARY KEY,
    ten_nguoi_dung NVARCHAR(100) NOT NULL,
	ten_dang_nhap VARCHAR(50) NOT NULL,
    mat_khau NVARCHAR(255) NOT NULL,
	email NVARCHAR(100) UNIQUE NOT NULL,
    admin INT NOT NULL DEFAULT 0, -- mặc định user
    ngay_dang_ky DATETIME DEFAULT GETDATE()
);

-- ===============================
-- Bảng Thể Loại
-- ===============================
CREATE TABLE TheLoai (
    ma_the_loai INT PRIMARY KEY IDENTITY(1,1),
    ten_the_loai NVARCHAR(100) NOT NULL
);

-- ===============================
-- Bảng Truyện Tranh
-- ===============================
CREATE TABLE TruyenTranh (
    ma_truyen INT PRIMARY KEY IDENTITY(1,1),
    ten_truyen NVARCHAR(200) NOT NULL,
    tac_gia NVARCHAR(100),
    mo_ta NVARCHAR(MAX),
    image NVARCHAR(255), -- đường dẫn ảnh bìa
    ngay_dang DATE DEFAULT GETDATE(),
    so_luot_xem INT DEFAULT 0,
    IsUnique BIT DEFAULT 0
);

-- ===============================
-- Bảng Liên kết Truyện - Thể loại (nhiều - nhiều)
-- ===============================
CREATE TABLE Truyen_TheLoai (
    ma_truyen INT FOREIGN KEY REFERENCES TruyenTranh(ma_truyen) ON DELETE CASCADE,
    ma_the_loai INT FOREIGN KEY REFERENCES TheLoai(ma_the_loai) ON DELETE CASCADE,
    PRIMARY KEY (ma_truyen, ma_the_loai)
);

-- ===============================
-- Bảng Chương
-- ===============================
CREATE TABLE Chuong (
    ma_chuong INT PRIMARY KEY IDENTITY(1,1),
    ma_truyen INT FOREIGN KEY REFERENCES TruyenTranh(ma_truyen) ON DELETE CASCADE,
    so_chuong INT NOT NULL,
    tieu_de NVARCHAR(200),
    ngay_dang DATE DEFAULT GETDATE()
);

-- ===============================
-- Bảng Trang Ảnh (1 chương nhiều ảnh)
-- ===============================
CREATE TABLE TrangAnh (
    ma_trang INT PRIMARY KEY IDENTITY(1,1),
    ma_chuong INT FOREIGN KEY REFERENCES Chuong(ma_chuong) ON DELETE CASCADE,
    so_trang INT NOT NULL,
    duong_dan NVARCHAR(255) NOT NULL
);

-- ===============================
-- Bảng Bình Luận
-- ===============================
CREATE TABLE BinhLuan (
    ma_binh_luan INT PRIMARY KEY IDENTITY(1,1),
    ma_nguoi_dung BIGINT FOREIGN KEY REFERENCES NguoiDung(ma_nguoi_dung),
    noi_dung NVARCHAR(MAX) NOT NULL,
    ngay_tao DATETIME DEFAULT GETDATE(),
	ma_binh_luan_cha INT NULL;
    loai_doi_tuong NVARCHAR(10) CHECK (loai_doi_tuong IN (N'Truyện',N'Chương')),
    ma_truyen INT NULL FOREIGN KEY REFERENCES TruyenTranh(ma_truyen),
    ma_chuong INT NULL FOREIGN KEY REFERENCES Chuong(ma_chuong)
);

-- ===============================
-- Bảng Đánh Giá
-- ===============================
CREATE TABLE DanhGia (
    ma_danh_gia INT PRIMARY KEY IDENTITY(1,1),
    ma_truyen INT FOREIGN KEY REFERENCES TruyenTranh(ma_truyen),
    ma_nguoi_dung BIGINT FOREIGN KEY REFERENCES NguoiDung(ma_nguoi_dung),
    ratings INT CHECK (ratings BETWEEN 1 AND 5),
    ngay_danh_gia DATETIME DEFAULT GETDATE()
);

-- ===============================
-- Bảng Thống Kê Truyện
-- ===============================
CREATE TABLE ThongKeTruyen (
    ma_thong_ke INT PRIMARY KEY IDENTITY(1,1),
    ma_truyen INT FOREIGN KEY REFERENCES TruyenTranh(ma_truyen),
    so_luot_xem INT DEFAULT 0,
    so_luot_xem_tuan INT DEFAULT 0,
    diem_trung_binh FLOAT DEFAULT 0, -- trung bình từ DanhGia
    la_doc_dao BIT DEFAULT 0
);

-- ===============================
-- Bảng Thư Viện (truyện đã lưu)
-- ===============================
CREATE TABLE ThuVien (
    ma_thu_vien INT PRIMARY KEY IDENTITY(1,1),
    ma_nguoi_dung BIGINT FOREIGN KEY REFERENCES NguoiDung(ma_nguoi_dung),
    ma_truyen INT FOREIGN KEY REFERENCES TruyenTranh(ma_truyen),
    ngay_luu DATETIME DEFAULT GETDATE(),
    UNIQUE (ma_nguoi_dung, ma_truyen)
);
