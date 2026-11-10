using System.Data;
using QuanLyTongHop.DAL;

namespace QuanLyTongHop.BUS
{
    public class KaraokeBUS
    {
        private KaraokeDAL dal = new KaraokeDAL();

        public DataTable GetDanhSachPhong() => dal.GetDanhSachPhong();
        public DataTable GetDanhSachDichVu() => dal.GetDanhSachDichVu();
        public bool MoPhong(int maPhong) => dal.MoPhong(maPhong);
        public bool ThanhToanKaraoke(int maPhong, out decimal tongTien) => dal.ThanhToanKaraoke(maPhong, out tongTien);
    }
}