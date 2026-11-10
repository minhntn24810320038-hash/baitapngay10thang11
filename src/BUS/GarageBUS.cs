using System.Data;
using QuanLyTongHop.DAL;

namespace QuanLyTongHop.BUS
{
    public class GarageBUS
    {
        private GarageDAL dal = new GarageDAL();

        public DataTable TraCuuXe(string bienSo) => dal.TraCuuXe(bienSo);
        public DataTable GetPhuTung() => dal.GetPhuTung();
        public bool LapPhieuSuaChua(string bienSo, string noiDung, DataTable dsPhuTung, out string message) 
            => dal.LapPhieuSuaChua(bienSo, noiDung, dsPhuTung, out message);
    }
}