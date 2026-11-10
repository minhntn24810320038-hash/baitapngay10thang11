-- 1. KÍCH HOẠT VÀ TẠO DATABASE
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'QuanLyTongHopDB')
BEGIN
    DROP DATABASE QuanLyTongHopDB;
END
GO

CREATE DATABASE QuanLyTongHopDB;
GO
USE QuanLyTongHopDB;
GO

-- =============================================
-- BÀI 6.3: HỆ THỐNG QUẢN LÝ DỰ ÁN & TASK (PROJECT TASK)
-- =============================================
CREATE TABLE DuAn (
    MaDuAn INT IDENTITY(1,1) PRIMARY KEY,
    TenDuAn NVARCHAR(150) NOT NULL,
    NgayBatDau DATETIME DEFAULT GETDATE(),
    NgayKetThuc DATETIME,
    TrangThai NVARCHAR(50) DEFAULT N'Đang chạy'
);

CREATE TABLE ThanhVien (
    MaThanhVien INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    ChuyenMon NVARCHAR(100)
);

CREATE TABLE CongViec (
    MaTask INT IDENTITY(1,1) PRIMARY KEY,
    MaDuAn INT FOREIGN KEY REFERENCES DuAn(MaDuAn) ON DELETE CASCADE,
    TenTask NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(MAX),
    HanNop DATETIME,
    DoUuTien NVARCHAR(20) DEFAULT N'Trung bình',
    TrangThai NVARCHAR(50) DEFAULT N'To Do'
);

CREATE TABLE PhanCong (
    MaTask INT FOREIGN KEY REFERENCES CongViec(MaTask) ON DELETE CASCADE,
    MaThanhVien INT FOREIGN KEY REFERENCES ThanhVien(MaThanhVien) ON DELETE CASCADE,
    NgayGiao DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (MaTask, MaThanhVien)
);

-- =============================================
-- BÀI 6.4: HỆ THỐNG QUẢN LÝ GARAGE Ô TÔ (GARAGE MANAGEMENT)
-- =============================================
CREATE TABLE KhachHangGarage (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    TenKH NVARCHAR(100) NOT NULL,
    DienThoai VARCHAR(15) UNIQUE,
    DiaChi NVARCHAR(200)
);

CREATE TABLE Xe (
    BienSo VARCHAR(20) PRIMARY KEY,
    MaKH INT FOREIGN KEY REFERENCES KhachHangGarage(MaKH) ON DELETE CASCADE,
    HieuXe NVARCHAR(50),
    NamSanXuat INT
);

CREATE TABLE PhuTung (
    MaPhuTung INT IDENTITY(1,1) PRIMARY KEY,
    TenPhuTung NVARCHAR(150) NOT NULL,
    DonGia DECIMAL(18,2) DEFAULT 0,
    SoLuongTon INT CHECK (SoLuongTon >= 0) DEFAULT 0
);

CREATE TABLE PhieuSuaChua (
    MaPhieu INT IDENTITY(1,1) PRIMARY KEY,
    BienSo VARCHAR(20) FOREIGN KEY REFERENCES Xe(BienSo) ON DELETE CASCADE,
    NgayNhan DATETIME DEFAULT GETDATE(),
    NoiDungSua NVARCHAR(MAX),
    TongTien DECIMAL(18,2) DEFAULT 0,
    TrangThai NVARCHAR(30) DEFAULT N'Đang sửa'
);

CREATE TABLE ChiTietSuaChua (
    MaPhieu INT FOREIGN KEY REFERENCES PhieuSuaChua(MaPhieu) ON DELETE CASCADE,
    MaPhuTung INT FOREIGN KEY REFERENCES PhuTung(MaPhuTung),
    SoLuong INT CHECK (SoLuong > 0),
    TienCong DECIMAL(18,2) DEFAULT 0,
    PRIMARY KEY (MaPhieu, MaPhuTung)
);

-- =============================================
-- BÀI 6.5: ĐẶT PHÒNG & DỊCH VỤ KARAOKE (KARAOKE MANAGEMENT)
-- =============================================
CREATE TABLE PhongHat (
    MaPhong INT IDENTITY(1,1) PRIMARY KEY,
    TenPhong NVARCHAR(50) NOT NULL,
    LoaiPhong NVARCHAR(30) DEFAULT N'Thường',
    GiaTheoGio DECIMAL(18,2) DEFAULT 100000,
    TrangThai NVARCHAR(20) DEFAULT N'Trống'
);

CREATE TABLE DichVu (
    MaDV INT IDENTITY(1,1) PRIMARY KEY,
    TenDV NVARCHAR(100) NOT NULL,
    DonGia DECIMAL(18,2) DEFAULT 0,
    TonKho INT DEFAULT 0
);

CREATE TABLE HoaDonKaraoke (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    MaPhong INT FOREIGN KEY REFERENCES PhongHat(MaPhong) ON DELETE CASCADE,
    GioVao DATETIME NOT NULL DEFAULT GETDATE(),
    GioRa DATETIME NULL,
    TongTienPhong DECIMAL(18,2) DEFAULT 0,
    TongTienDichVu DECIMAL(18,2) DEFAULT 0,
    TrangThai BIT DEFAULT 0 -- 0: Chưa thanh toán, 1: Đã thanh toán
);

CREATE TABLE ChiTietDichVu (
    MaHD INT FOREIGN KEY REFERENCES HoaDonKaraoke(MaHD) ON DELETE CASCADE,
    MaDV INT FOREIGN KEY REFERENCES DichVu(MaDV),
    SoLuong INT CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) DEFAULT 0,
    PRIMARY KEY (MaHD, MaDV)
);
GO

-- =============================================
-- DỮ LIỆU MẪU BAN ĐẦU (SEED DATA FOR TESTING)
-- =============================================

-- 1. Mẫu Bài 6.3 (Dự án & Task)
INSERT INTO DuAn (TenDuAn, NgayBatDau, NgayKetThuc, TrangThai) VALUES
(N'Xây dựng Website E-Commerce', '2026-01-01', '2026-12-31', N'Đang chạy'),
(N'Phát triển App Mobile WinForms 3-Tier', '2026-02-15', '2026-11-30', N'Đang chạy');

INSERT INTO ThanhVien (HoTen, Email, ChuyenMon) VALUES
(N'Nguyễn Văn A', 'a.nguyen@gmail.com', N'Backend Developer'),
(N'Trần Thị B', 'b.tran@gmail.com', N'UI/UX Designer');

INSERT INTO CongViec (MaDuAn, TenTask, MoTa, HanNop, DoUuTien, TrangThai) VALUES
(1, N'Thiết kế CSDL SQL Server', N'Tạo các bảng và khóa ngoại', '2026-03-01', N'Cao', N'Done'),
(1, N'Viết API Đăng nhập', N'Dùng JWT Token', '2026-04-10', N'Cao', N'In Progress'),
(1, N'Fix lỗi Trễ hạn Báo cáo', N'Kiểm tra lại SqlDataReader', '2025-10-15', N'Cao', N'To Do'); -- Task trễ hạn test warning

-- 2. Mẫu Bài 6.4 (Garage Ô tô)
INSERT INTO KhachHangGarage (TenKH, DienThoai, DiaChi) VALUES
(N'Phạm Minh Tuấn', '0912345678', N'123 Nguyễn Trãi, Hà Nội'),
(N'Lê Hoàng Nam', '0987654321', N'456 Cầu Giấy, Hà Nội');

INSERT INTO Xe (BienSo, MaKH, HieuXe, NamSanXuat) VALUES
('30A-123.45', 1, N'Toyota Camry', 2021),
('29B-999.99', 2, N'VinFast VF8', 2023);

INSERT INTO PhuTung (TenPhuTung, DonGia, SoLuongTon) VALUES
(N'Lọc nhớt Động cơ Toyota', 180000, 30),
(N'Bugi Iridium NGK', 250000, 50),
(N'Bộ Má phanh trước VinFast', 850000, 10),
(N'Dầu động cơ Castrol Edge 4L', 480000, 20);

-- 3. Mẫu Bài 6.5 (Karaoke)
INSERT INTO PhongHat (TenPhong, LoaiPhong, GiaTheoGio, TrangThai) VALUES
(N'Phòng 101', N'Thường', 80000, N'Trống'),
(N'Phòng 102', N'Thường', 80000, N'Trống'),
(N'Phòng VIP 01', N'VIP', 150000, N'Trống'),
(N'Phòng VIP 02', N'VIP', 200000, N'Trống');

INSERT INTO DichVu (TenDV, DonGia, TonKho) VALUES
(N'Bia Heineken (Lon)', 25000, 150),
(N'Nước ngọt Cocacola', 15000, 200),
(N'Đĩa Trái cây Thập cẩm', 120000, 40),
(N'Khăn lạnh', 5000, 500);
GO