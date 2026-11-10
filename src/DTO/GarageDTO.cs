using System;

namespace QuanLyTongHop.DTO
{
    public class KhachHangGarageDTO
    {
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
    }

    public class XeDTO
    {
        public string BienSo { get; set; }
        public int MaKH { get; set; }
        public string HieuXe { get; set; }
        public int NamSanXuat { get; set; }
    }

    public class PhuTungDTO
    {
        public int MaPhuTung { get; set; }
        public string TenPhuTung { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuongTon { get; set; }
    }
}